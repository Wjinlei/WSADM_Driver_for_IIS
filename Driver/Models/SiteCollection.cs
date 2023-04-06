using Microsoft.Web.Administration;
using System.Collections;
using WSADM;
using WSADM.Interfaces;

namespace Driver.Models;

public class SiteCollection : ISiteCollection<ISite>
{
    // Private
    private readonly ServerManager _serverManager;
    private readonly List<ISite> _sites;

    // Public
    public int Count => _sites.Count;

    // Indexer
    public ISite this[int index] { get => _sites[index]; }
    public ISite? this[string name] => _sites.Find(s => s.Name == name);

    // Constructor
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

    // Methods
    public Result Add(ISite site)
    {
        // Check parameter
        if (site.Bindings.Count < 1)
            return Result.Error(new ArgumentException("The binding information cannot be empty"));
        if (string.IsNullOrWhiteSpace(site.Name))
            return Result.Error(new ArgumentException("The site name cannot be empty"));
        if (string.IsNullOrWhiteSpace(site.PhysicalPath))
            return Result.Error(new ArgumentException("The physical path cannot be empty"));
        if (_sites.Contains(site))
            return Result.Error(new ArgumentException("This site already exists"));

        _sites.Add(site);
        return Result.Ok;
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

    public Result Add(string name, string physicalPath, List<string> bindings)
    {
        var bindingInformationCollection = new BindingInformationCollection(this);
        bindings.ForEach(element =>
        {
            var binding = element.Split(':');
            var domainNameOrDefault = binding.ElementAtOrDefault(0) ?? "";
            var portOrDefault = binding.ElementAtOrDefault(1) ?? "80";

            // Add bindingInformation
            bindingInformationCollection.Add(
                new BindingInformation(
                    domainNameOrDefault,
                    Util.Convert.ToInt32OrDefault(portOrDefault, 80)));
        });
        return Add(new Site(name, physicalPath, bindingInformationCollection));
    }

    // Implementation List
    public ISite? Find(Predicate<ISite> match)
    {
        return _sites.Find(match);
    }

    public bool Contains(ISite site)
    {
        return _sites.Contains(site);
    }

    public bool Contains(string name)
    {
        var site = _sites.Find(site => site.Name == name);
        if (site != null) return true;
        return false;
    }

    public void Remove(ISite site)
    {
        _sites.Remove(site);
    }

    public void Remove(string name)
    {
        var site = _sites.Find(site => site.Name == name);
        if (site != null)
            _sites.Remove(site);
    }

    public void Clear()
    {
        _sites.Clear();
    }

    // Implementation iterator
    public IEnumerator<ISite> GetEnumerator()
    {
        return _sites.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
