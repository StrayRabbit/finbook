using System.Net;

namespace Project.API.Dto
{
    public class ServiceDiscoveryOptions
    {
        public string ServiceName { get; set; }
        public ConsulOptions Consul { get; set; }
    }

    public class ConsulOptions
    {
        public string HttpEndpoint { get; set; }
        public DnsEndPoint DnsEndPoint { get; set; }
    }

    public class DnsEndPoint
    {
        public string Address { get; set; }
        public int Port { get; set; }

        public IPEndPoint ToIpEndPoint()
        {
            return new IPEndPoint(IPAddress.Parse(Address), Port);
        }
    }
}
