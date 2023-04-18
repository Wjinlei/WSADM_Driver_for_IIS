﻿using Microsoft.Web.Administration;
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

        // Load Sites
        foreach (var site in _serverManager.Sites)
        {
            var bindingInformationCollection = new BindingInformationCollection();
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
        var bindingInformationCollection = new BindingInformationCollection();
        var result = bindingInformationCollection.Add(binding);
        if (!result.Success)
            return result;

        return Add(new Site(name, physicalPath, bindingInformationCollection));
    }

    public Result Add(string name, string physicalPath, List<string> bindings)
    {
        var bindingInformationCollection = new BindingInformationCollection();
        bindings.ForEach(element => bindingInformationCollection.Add(element));
        return Add(new Site(name, physicalPath, bindingInformationCollection));
    }

    // Implementation List
    public void Clear()
    {
        _sites.Clear();
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

    public bool Contains(ISite site)
    {
        return _sites.Contains(site);
    }

    public bool Contains(string name)
    {
        return _sites.Find(site => site.Name == name) != null;
    }

    public ISite? Find(Predicate<ISite> match)
    {
        return _sites.Find(match);
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
