using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Recommend.API.Dtos;
using DnsClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Resilience;

namespace Recommend.API.Services
{
    public class UserService : Recommend.API.Services.IUserService
    {
        private IHttpClient _httpClient;
        private string _userServiceUrl;
        private ILogger<UserService> _logger;

        public UserService(IHttpClient httpClient,
            IOptions<ServiceDiscoveryOptions> serviceDiscoveryOptions,
            IDnsQuery dnsQuery,
            ILogger<UserService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            var result = dnsQuery.ResolveService("service.consul",
                serviceDiscoveryOptions.Value.UserServiceName);
            var addressList = result.First().AddressList;
            var address = addressList.Any() ? addressList.First().ToString() : result.First().HostName.TrimEnd('.');
            var port = result.First().Port;
            _userServiceUrl = $"http://{address}:{port}/";
        }


        public async Task<int> CheckOrCreate(string phone)
        {
            var form = new Dictionary<string, string> { { "phone", phone } };
            var content = new FormUrlEncodedContent(form);

            try
            {
                var response = await _httpClient.PostAsync(_userServiceUrl + "/api/users/check-or-create", content);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var userId = await response.Content.ReadAsStringAsync();
                    int.TryParse(userId, out int intUserId);
                    _logger.LogTrace($"Completed CheckOrCreate with userId:{intUserId}");
                    return intUserId;
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"CheckOrCreate 在重试之后失效，{ex.Message + ex.StackTrace}");
                throw ex;
            }

            return 0;
        }

        public async Task<UserIdentity> GetBaseUserInfoAsync(int userId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _httpClient.GetStringAsync(_userServiceUrl + "api/users/baseinfo/" + userId);

                if (!string.IsNullOrWhiteSpace(response))
                {
                    var userInfo = JsonConvert.DeserializeObject<UserIdentity>(response);
                    _logger.LogTrace($"Completed GetBaseUserInfoAsync with userId:{userId}");
                    return userInfo;
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"GetBaseUserInfoAsync 在重试之后失效，{ex.Message + ex.StackTrace}");
                throw ex;
            }

            return null;
        }
    }
}