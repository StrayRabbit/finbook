using Resilience;

namespace Recommend.API.Infrastructure
{
    public interface IResilientHttpClientFactory
    {
        ResilienceHttpClient CreateResilientHttpClient();
    }
}