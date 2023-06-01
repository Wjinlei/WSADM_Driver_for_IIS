using WSADM.Interfaces;

namespace Driver.Models;

public class Site : ISite
{
    private readonly Microsoft.Web.Administration.Site _site;

    public string Name
    {
        get => _site.Name;
        set => _site.Name = value;
    }

    public string PhysicalPath
    {
        get => _site.Applications["/"].VirtualDirectories["/"].PhysicalPath;
        set => _site.Applications["/"].VirtualDirectories["/"].PhysicalPath = value;
    }

    public IBindingInformationCollection Bindings => new BindingCollection(_site.Bindings);

    public ISiteLimits Limits => new SiteLimits(_site.Limits);

    public ObjectState State
    {
        get
        {
            return _site.State switch
            {
                Microsoft.Web.Administration.ObjectState.Starting => ObjectState.Starting,
                Microsoft.Web.Administration.ObjectState.Started => ObjectState.Started,
                Microsoft.Web.Administration.ObjectState.Stopping => ObjectState.Stopping,
                Microsoft.Web.Administration.ObjectState.Stopped => ObjectState.Stopped,
                _ => ObjectState.Unknown,
            };
        }
    }

    public Site(Microsoft.Web.Administration.Site site)
    {
        _site = site;
    }

    public void Start()
    {
        _site.Start();
    }

    public void Stop()
    {
        _site.Stop();
    }

    /// <summary>
    /// Override
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        return _site.Equals(obj);
    }

    /// <summary>
    /// Override
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return _site.GetHashCode();
    }
}
