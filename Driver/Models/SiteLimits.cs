using WSADM.Interfaces;

namespace Driver.Models;

public class SiteLimits : ISiteLimits
{
    public TimeSpan ConnectionTimeout { get; set; }
    public long MaxUrlSegments { get; set; }
    public long MaxConnections { get; set; }
    public long MaxBandwidth { get; set; }

    public SiteLimits(TimeSpan connectionTimeout, long maxUrlSegments, long maxConnections, long maxBandwidth)
    {
        ConnectionTimeout = connectionTimeout;
        MaxUrlSegments = maxUrlSegments;
        MaxConnections = maxConnections;
        MaxBandwidth = maxBandwidth;
    }
}
