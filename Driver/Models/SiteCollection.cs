using Microsoft.Web.Administration;
using System.Collections;
using WSADM;
using WSADM.Interfaces;

namespace Driver.Models;

public class SiteCollection : ISiteCollection<ISite>
{
    // Private
    private readonly List<ISite> _sites;
    private readonly ServerManager _serverManager;

    // Public
    public int Count => _sites.Count;

    // Indexer
    public ISite this[int index] => _sites[index];
    public ISite? this[string name] => _sites.Find(s => s.Name == name);

    // Constructor
    public SiteCollection(ServerManager serverManager)
    {
        _sites = new List<ISite>();
        _serverManager = serverManager;

        // Load all sites from ServerManager
        foreach (var site in _serverManager.Sites)
        {
            // Load the site binding information
            var bindingCollection = new BindingCollection(this);
            foreach (var binding in site.Bindings)
            {
                bindingCollection.Add(
                    new Binding(
                        binding.EndPoint.Address.ToString(),
                        binding.Host,
                        binding.EndPoint.Port)
                    );
            }

            // Load the site into its own object
            var _site = new Site(
                site.Name,
                site.Applications["/"].VirtualDirectories["/"].PhysicalPath,
                bindingCollection);

            // Mapping site properties
            _site.Limits.ConnectionTimeout = site.Limits.ConnectionTimeout;
            _site.Limits.MaxUrlSegments = site.Limits.MaxUrlSegments;
            _site.Limits.MaxConnections = site.Limits.MaxConnections;
            _site.Limits.MaxBandwidth = site.Limits.MaxBandwidth;

            _sites.Add(_site);
        }
    }

    // Methods
    public Result Add(ISite site)
    {
        // Check parameter
        var result = this.Check(site);
        if (result.Success)
            _sites.Add(site);
        return result;
    }

    public Result Add(
        string name,
        string physicalPath,
        IBindingInformationCollection bindings)
    {
        return Add(new Site(name, physicalPath, bindings));
    }

    public Result Add(string name, string physicalPath, int port)
    {
        return Add(name, physicalPath, ":" + port);
    }

    public Result Add(string name, string physicalPath, string domain, int port)
    {
        return Add(name, physicalPath, domain + ":" + port);
    }

    public Result Add(string name, string physicalPath, string binding)
    {
        var bindingCollection = new BindingCollection(this);
        var result = bindingCollection.Add(binding);
        if (!result.Success)
            return result;

        return Add(new Site(name, physicalPath, bindingCollection));
    }

    public Result Add(string name, string physicalPath, List<string> bindings)
    {
        var bindingCollection = new BindingCollection(this);
        foreach (var binding in bindings)
        {
            var result = bindingCollection.Add(binding);
            if (!result.Success)
                return result;
        }
        return Add(new Site(name, physicalPath, bindingCollection));
    }

    /// <summary>
    /// Removes all elements from List<T>.
    /// </summary>
    public void Clear()
    {
        _sites.Clear();
    }

    /// <summary>
    /// Find and Removes the first match of a particular object from List<T>.
    /// </summary>
    public void Remove(string name)
    {
        var site = _sites.Find(site => site.Name == name);
        if (site != null)
            _sites.Remove(site);
    }

    /// <summary>
    /// Removes the first match of a particular object from List<T>.
    /// </summary>
    public void Remove(ISite site)
    {
        _sites.Remove(site);
    }

    /// <summary>
    /// Determines if a site is in List<T>.
    /// </summary>
    public bool Contains(string name)
    {
        return _sites.Find(site => site.Name == name) != null;
    }

    /// <summary>
    /// Determines if a site is in List<T>.
    /// </summary>
    public bool Contains(ISite site)
    {
        return _sites.Contains(site);
    }

    /// <summary>
    /// Searches for site that matches the condition defined by the specified predicate and returns the first matching element in the entire List<T>.
    /// </summary>
    public ISite? Find(Predicate<ISite> match)
    {
        return _sites.Find(match);
    }

    /// <summary>
    /// Implementation iterator
    /// </summary>
    /// <returns></returns>
    public IEnumerator<ISite> GetEnumerator()
    {
        return _sites.GetEnumerator();
    }

    /// <summary>
    /// Implementation iterator
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
