using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DnsClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Recommend.API.Dtos;
using Resilience;

namespace Recommend.API.Services
{
    public class ContactService : IContactService
    {
        private IHttpClient _httpClient;
        private string _contactServiceUrl;
        private ILogger<ContactService> _logger;

        public ContactService(IHttpClient httpClient,
            IOptions<ServiceDiscoveryOptions> serviceDiscoveryOptions,
            IDnsQuery dnsQuery,
            ILogger<ContactService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            var result = dnsQuery.ResolveService("service.consul",
                serviceDiscoveryOptions.Value.ContactServiceName);
            var addressList = result.First().AddressList;
            var address = addressList.Any() ? addressList.First().ToString() : result.First().HostName.TrimEnd('.');
            var port = result.First().Port;
            _contactServiceUrl = $"http://{address}:{port}/";
        }

        public async Task<List<Contact>> GetContactsByUserId(int userId)
        {
            try
            {
                var response = await _httpClient.GetStringAsync(_contactServiceUrl + "api/contacts/" + userId);

                if (!string.IsNullOrWhiteSpace(response))
                {
                    var list = JsonConvert.DeserializeObject<List<Contact>>(response);
                    _logger.LogTrace($"Completed GetContactsByUserId with userId:{userId}");
                    return list;
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"GetContactsByUserId 在重试之后失效，{ex.Message + ex.StackTrace}");
                throw ex;
            }

            return null;
        }
    }
}
