using System.Collections;
using WSADM;
using WSADM.Interfaces;

namespace Driver.Models;

public class BindingInformationCollection : IBindingInformationCollection
{
    private readonly List<IBindingInformation> _list;

    private readonly SiteCollection _sites;

    public int Count => _list.Count;

    public IBindingInformation? this[string bindingInformationStr]
    {
        get
        {
            return _list.Find(binding => binding.ToString() == bindingInformationStr);
        }
    }

    public IBindingInformation this[int index] => _list[index];

    public BindingInformationCollection(SiteCollection sites)
    {
        _list = new List<IBindingInformation>();
        _sites = sites;
    }

    public Result Add(string domain, int port)
    {
        return Add(new BindingInformation(domain, port));
    }

    public Result Add(int port)
    {
        return Add(new BindingInformation("", port));
    }

    public Result Add(IBindingInformation bindingInformation)
    {
        var result = Check(bindingInformation);
        if (!result.Success)
            return result;

        _list.Add(bindingInformation);
        return Result.Ok;
    }

    public void Clear()
    {
        _list.Clear();
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

    public IBindingInformation? Find(Predicate<IBindingInformation> match)
    {
        return _list.Find(match);
    }

    public IEnumerator<IBindingInformation> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

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

    /// <summary>
    /// Check binding information
    /// </summary>
    /// <param name="bindingInformation">IBindingInformation instance.</param>
    /// <returns></returns>
    private Result Check(IBindingInformation bindingInformation)
    {
        // Check port
        if (bindingInformation.Port < 1 || bindingInformation.Port > 65535)
            return Result.Error(
                new ArgumentException(nameof(bindingInformation.Port))
                );

        // Check binding information exists in self list
        if (_list.Contains(bindingInformation))
            return Result.Error(
                new Exception("This binding information already exists")
                );

        // Check binding information exists in all site
        if (_sites.Find(site => site.Bindings.Contains(bindingInformation)) != null)
            return Result.Error(
                new Exception("This binding information already exists")
                );

        return Result.Ok;
    }
}
