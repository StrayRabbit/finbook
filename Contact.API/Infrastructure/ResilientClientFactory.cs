using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Polly;
using Resilience;

namespace Contact.API.Infrastructure
{
    public class ResilientClientFactory : IResilientHttpClientFactory
    {
        private readonly ILogger<ResilienceHttpClient> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        //重试次数
        private int _retryCount;

        //熔断之前允许的异常次数
        private int _exceptionCountAllowedBeforeBreaking;

        public ResilientClientFactory(ILogger<ResilienceHttpClient> logger, IHttpContextAccessor httpContextAccessor, int retryCount, int exceptionCountAllowedBeforeBreaking)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _retryCount = retryCount;
            _exceptionCountAllowedBeforeBreaking = exceptionCountAllowedBeforeBreaking;
        }

        public ResilienceHttpClient CreateResilientHttpClient() =>
            new ResilienceHttpClient("contact_api",origin => CreatePolicy(), _logger, _httpContextAccessor);

        private Policy[] CreatePolicy()
        {
            return new Policy[]
                {
                Policy.Handle<HttpRequestException>()
                    .WaitAndRetryAsync(
                        // number of retries
                        _retryCount,
                        // exponential backofff
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        // on retry
                        (exception, timeSpan, retryCount, context) =>
                        {
                            var msg = $"第 {retryCount} 次重试 " +
                                      $"of {context.PolicyKey} " +
                                      // $"at {context.ExecutionKey}, " +
                                      $"due to: {exception}.";
                            _logger.LogWarning(msg);
                            _logger.LogDebug(msg);
                        }),
                Policy.Handle<HttpRequestException>()
                    .CircuitBreakerAsync( 
                        // number of exceptions before breaking circuit
                        _exceptionCountAllowedBeforeBreaking,
                        // time circuit opened before retry
                        TimeSpan.FromMinutes(1),
                        (exception, duration) =>
                        {
                            // on circuit opened
                            _logger.LogTrace("熔断器打开");
                        },
                        () =>
                        {
                            // on circuit closed
                            _logger.LogTrace("熔断器关闭");
                        })
                };

        }
    }
}
