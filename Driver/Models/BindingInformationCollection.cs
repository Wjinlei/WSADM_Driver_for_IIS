using System.Collections;
using WSADM;
using WSADM.Interfaces;

namespace Driver.Models;

public class BindingInformationCollection : IBindingInformationCollection
{
    // Private
    private readonly List<IBindingInformation> _list;
    private readonly SiteCollection _sites;

    // Public
    public int Count => _list.Count;

    // Indexer
    public IBindingInformation this[int index] => _list[index];
    public IBindingInformation? this[string bindingInformationStr] => _list.Find(binding => binding.ToString() == bindingInformationStr);

    // Constructor
    public BindingInformationCollection(SiteCollection sites)
    {
        _list = new List<IBindingInformation>();
        _sites = sites;
    }

    // Methods
    public Result Add(IBindingInformation bindingInformation)
    {
        // Check parameter
        if (bindingInformation.Port < 1 || bindingInformation.Port > 65535)
            return Result.Error(new ArgumentException(nameof(bindingInformation.Port)));
        if (_list.Contains(bindingInformation))
            return Result.Error(new ArgumentException("This binding information already exists"));
        if (_sites.Any(site => site.Bindings.Contains(bindingInformation)))
            return Result.Error(new ArgumentException("This binding information already exists"));

        _list.Add(bindingInformation);
        return Result.Ok;
    }

    public Result Add(string domain, int port)
    {
        return Add(new BindingInformation(domain, port));
    }

    public Result Add(int port)
    {
        return Add(new BindingInformation("", port));
    }

    // Implementation List
    public IBindingInformation? Find(Predicate<IBindingInformation> match)
    {
        return _list.Find(match);
    }

    public bool Contains(IBindingInformation bindingInformation)
    {
        return _list.Contains(bindingInformation);
    }

    public bool Contains(string bindingInformationStr)
    {
        var binding = _list.Find(binding => binding.ToString() == bindingInformationStr);
        if (binding != null) return true;
        return false;
    }

    public void Remove(IBindingInformation bindingInformation)
    {
        _list.Remove(bindingInformation);
    }

    public void Remove(string bindingInformationStr)
    {
        var binding = _list.Find(binding => binding.ToString() == bindingInformationStr);
        if (binding != null)
            _ = _list.Remove(binding);
    }

    public void Clear()
    {
        _list.Clear();
    }

    // Implementation iterator
    public IEnumerator<IBindingInformation> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    // Implementation comparator
    public override bool Equals(object? obj)
    {
        return obj is BindingInformationCollection collection &&
               EqualityComparer<List<IBindingInformation>>.Default.Equals(_list, collection._list) &&
               Count == collection.Count;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_list, Count);
    }
}
