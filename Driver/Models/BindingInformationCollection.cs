using System.Collections;
using WSADM;
using WSADM.Interfaces;

namespace Driver.Models;

public class BindingInformationCollection : IBindingInformationCollection
{
    private readonly List<IBindingInformation> _list;

    private readonly SiteCollection _sites;

    public int Count => _list.Count;

    public BindingInformationCollection(SiteCollection sites)
    {
        _list = new List<IBindingInformation>();
        _sites = sites;
    }

    public IBindingInformation this[int index] => _list[index];

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
        if (result.Success)
            _list.Add(bindingInformation);

        return result;
    }

    public void Clear()
    {
        _list.Clear();
    }

    public void Remove(IBindingInformation bindingInformation)
    {
        _list.Remove(bindingInformation);
    }

    public bool Contains(IBindingInformation bindingInformation)
    {
        return _list.Contains(bindingInformation);
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
                new ArgumentException(nameof(bindingInformation.Port)));

        // Check binding information exists in self list
        if (_list.Contains(bindingInformation))
            return Result.Error(
                new Exception("This binding information already exists"));

        // Check binding information exists in all site
        if (_sites.FirstOrDefault(site => site.Bindings.Contains(bindingInformation)) != null)
            return Result.Error(
                new Exception("This binding information already exists"));

        return Result.Ok;
    }
}
