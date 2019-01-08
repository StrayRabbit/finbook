using Resilience;

namespace Contact.API.Infrastructure
{
    public interface IResilientHttpClientFactory
    {
        ResilienceHttpClient CreateResilientHttpClient();
    }
}