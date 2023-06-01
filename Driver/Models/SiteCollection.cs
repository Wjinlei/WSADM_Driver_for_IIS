using System.Collections;
using WSADM;
using WSADM.Interfaces;

namespace Driver.Models;

public class SiteCollection : ISiteCollection<ISite>
{
    // Private
    private readonly Microsoft.Web.Administration.SiteCollection _siteCollection;

    // Public
    public int Count => _siteCollection.Count;

    // Indexer
    public ISite this[int index]
    {
        get
        {
            return new Site(_siteCollection[index]);
        }
    }

    public ISite? this[string name]
    {
        get
        {
            var site = _siteCollection.FirstOrDefaultEx(name);
            return site != null ? new Site(site) : null;
        }
    }

    // Constructor
    public SiteCollection(Microsoft.Web.Administration.SiteCollection siteCollection)
    {
        _siteCollection = siteCollection;
    }

    // Methods
    public Result Add(
        string name,
        string physicalPath,
        IBindingInformationCollection bindings)
    {
        var _site = _siteCollection.CreateSiteDefaults(name, physicalPath);
        var result = _site.Bindings.TryAddCollection(bindings);
        if (!result.Success)
            return result;

        return _siteCollection.TryAdd(_site);
    }

    public Result Add(ISite site)
    {
        return Add(site.Name, site.PhysicalPath, site.Bindings);
    }

    public Result Add(string name, string physicalPath, string binding)
    {
        var _site = _siteCollection.CreateSiteDefaults(name, physicalPath);
        var result = _site.Bindings.TryAdd(binding);
        if (!result.Success)
            return result;

        return _siteCollection.TryAdd(_site);
    }

    public Result Add(string name, string physicalPath, string domain, int port)
    {
        return Add(name, physicalPath, domain + ":" + port);
    }

    public Result Add(string name, string physicalPath, List<string> bindings)
    {
        var _site = _siteCollection.CreateSiteDefaults(name, physicalPath);
        var result = _site.Bindings.TryAddCollection(bindings);
        if (!result.Success)
            return result;

        return _siteCollection.TryAdd(_site);
    }

    /// <summary>
    /// Removes all elements from List<T>.
    /// </summary>
    public void Clear()
    {
        _siteCollection.Clear();
    }

    /// <summary>
    /// Find and Removes the first match of a particular object from List<T>.
    /// </summary>
    public void Remove(string name)
    {
        var _site = _siteCollection.FirstOrDefaultEx(name);
        if (_site != null)
            _siteCollection.Remove(_site);
    }

    /// <summary>
    /// Removes the first match of a particular object from List<T>.
    /// </summary>
    public void Remove(ISite site)
    {
        Remove(site.Name);
    }

    /// <summary>
    /// Determines if a site is in List<T>.
    /// </summary>
    public bool Contains(string name)
    {
        return _siteCollection.FirstOrDefaultEx(name) != null;
    }

    /// <summary>
    /// Determines if a site is in List<T>.
    /// </summary>
    public bool Contains(ISite site)
    {
        return Contains(site.Name);
    }

    /// <summary>
    /// Implementation iterator
    /// </summary>
    /// <returns></returns>
    public IEnumerator<ISite> GetEnumerator()
    {
        return _siteCollection.Select(site => new Site(site)).ToList().GetEnumerator();
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
