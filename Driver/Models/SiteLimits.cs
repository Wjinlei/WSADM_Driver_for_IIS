using WSADM.Interfaces;

namespace Driver.Models;

public class SiteLimits : ISiteLimits
{
    private readonly Microsoft.Web.Administration.SiteLimits _siteLimits;

    public long MaxUrlSegments
    {
        get => _siteLimits.MaxUrlSegments;
        set => _siteLimits.MaxUrlSegments = value;
    }

    public long MaxConnections
    {
        get => _siteLimits.MaxConnections;
        set => _siteLimits.MaxConnections = value;
    }

    public long MaxBandwidth
    {
        get => _siteLimits.MaxBandwidth;
        set => _siteLimits.MaxBandwidth = value;
    }

    public TimeSpan ConnectionTimeout
    {
        get => _siteLimits.ConnectionTimeout;
        set => _siteLimits.ConnectionTimeout = value;
    }

    public SiteLimits(Microsoft.Web.Administration.SiteLimits siteLimits)
    {
        _siteLimits = siteLimits;
    }
}
