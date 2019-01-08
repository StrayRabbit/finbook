using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DnsClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Resilience;
using User.Identity.Dtos;

namespace User.Identity.Services
{
    public class UserService : IUserService
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
            _userServiceUrl = $"http://{address}:{port}";
        }


        public async Task<UserInfo> CheckOrCreate(string phone)
        {
            Dictionary<string, string> form = new Dictionary<string, string> { { "phone", phone } };
            var content = new FormUrlEncodedContent(form);
            try
            {
                var response = await _httpClient.PostAsync(_userServiceUrl + "/api/users/check-or-create", content);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var userInfo = JsonConvert.DeserializeObject<UserInfo>(result);

                    _logger.LogTrace($"Completed CheckOrCreate with userId:{userInfo?.UserId}");
                    return userInfo;
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"CheckOrCreate 在重试之后失效，{ex.Message + ex.StackTrace}");
                throw ex;
            }

            return null;
        }
    }
}