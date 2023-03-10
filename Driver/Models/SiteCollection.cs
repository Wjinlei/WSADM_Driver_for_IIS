using Microsoft.Web.Administration;
using System.Collections;
using WSADM;
using WSADM.Interfaces;

namespace Driver.Models;

public class SiteCollection : ISiteCollection<ISite>
{
    private readonly ServerManager _serverManager;

    private readonly List<ISite> _sites;

    public ISite this[string name]
    {
#pragma warning disable CS8603 // 可能返回 null 引用。
        get => _sites.FirstOrDefault(s => s.Name == name);
#pragma warning restore CS8603 // 可能返回 null 引用。
    }

    public ISite this[int index] { get => _sites[index]; }

    public int Count => _sites.Count;

    public SiteCollection(ServerManager serverManager)
    {
        _serverManager = serverManager;
        _sites = new List<ISite>();

        // Load Sites
        foreach (var site in _serverManager.Sites)
        {
            var bindingInformationCollection = new BindingInformationCollection(this);
            foreach (var binding in site.Bindings)
            {
                bindingInformationCollection.Add(new BindingInformation(binding.Host, binding.EndPoint.Port));
            }
            var s = new Site(site.Name, site.Applications["/"].VirtualDirectories["/"].PhysicalPath, bindingInformationCollection);
            _sites.Add(s);
        }
    }

    public Result Add(ISite site)
    {
        var result = Check(site);
        if (result.Success)
            _sites.Add(site);

        return result;
    }

    public Result Add(string name, string physicalPath, int port)
    {
        var bindings = new BindingInformationCollection(this);
        var result = bindings.Add(new BindingInformation("", port));
        if (!result.Success)
            return result;

        return Add(new Site(name, physicalPath, bindings));
    }

    public Result Add(string name, string physicalPath, string domain, int port)
    {
        var bindings = new BindingInformationCollection(this);
        var result = bindings.Add(new BindingInformation(domain, port));
        if (!result.Success)
            return result;

        return Add(new Site(name, physicalPath, bindings));
    }

    public Result Add(string name, string physicalPath, IBindingInformationCollection bindings)
    {
        return Add(new Site(name, physicalPath, bindings));
    }

    public void Clear()
    {
        _sites.Clear();
    }

    public bool Contains(ISite site)
    {
        return _sites.Contains(site);
    }

    public void Remove(ISite site)
    {
        _sites.Remove(site);
    }

    public IEnumerator<ISite> GetEnumerator()
    {
        return _sites.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public Result Check(ISite site)
    {
        if (site.Bindings.Count == 0)
            return Result.Error(new Exception("The binding information cannot be empty"));
        if (site.Name == "")
            return Result.Error(new Exception("The site name cannot be empty"));
        if (site.PhysicalPath == "")
            return Result.Error(new Exception("The physical path cannot be empty"));

        return Result.Ok;
    }
}
