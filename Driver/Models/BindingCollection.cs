using System.Collections;
using WSADM;
using WSADM.Interfaces;

namespace Driver.Models;

public class BindingCollection : IBindingInformationCollection
{
    // Private
    private readonly List<IBindingInformation> _list;
    private readonly ISiteCollection<ISite> _sites;

    // Public
    public int Count => _list.Count;

    // Indexer
    public IBindingInformation this[int index] => _list[index];
    public IBindingInformation? this[string bindingInformation] => _list.Find(binding =>
        binding.BindingInformation == bindingInformation || binding.EndPoint == bindingInformation);

    // Constructor
    public BindingCollection(ISiteCollection<ISite> sites)
    {
        _list = new List<IBindingInformation>();
        _sites = sites;
    }

    // Methods
    // Check parameter
    public Result Add(IBindingInformation bindingInformation)
    {
        var result = this.Check(bindingInformation, _sites);
        if (result.Success)
            _list.Add(bindingInformation);
        return result;
    }

    public Result Add(int port)
    {
        return Add(new Binding(port));
    }

    public Result Add(string domain, int port)
    {
        return Add(new Binding(domain, port));
    }

    public Result Add(string ipAddr, string domain, int port)
    {
        return Add(new Binding(ipAddr, domain, port));
    }

    /// <summary>
    /// Adding binding information
    /// </summary>
    /// <param name="bindingInformation"></param>
    /// <returns></returns>
    public Result Add(string bindingInformation)
    {
        var binding = bindingInformation.Split(":");

        var ipAddrOrDefault = binding.ElementAtOrDefault(binding.Length > 2 ? 0 : -1) ?? "0.0.0.0";
        var domainNameOrDefault = binding.ElementAtOrDefault(binding.Length > 2 ? 2 : 0) ?? "";
        var portOrDefault = binding.ElementAtOrDefault(1) ?? "80";

        return Add(new Binding(
            ipAddrOrDefault,
            domainNameOrDefault,
            Mojito.Convert.ToInt32OrDefault(portOrDefault, 80)));
    }

    /// <summary>
    /// Removes all elements from List<T>.
    /// </summary>
    public void Clear()
    {
        _list.Clear();
    }

    /// <summary>
    /// Find and Removes the first match of a particular object from List<T>.
    /// </summary>
    public void Remove(string bindingInformation)
    {
        var binding = _list.Find(binding
            => binding.BindingInformation == bindingInformation
            || binding.EndPoint == bindingInformation);

        if (binding != null)
            _list.Remove(binding);
    }

    /// <summary>
    /// Removes the first match of a particular object from List<T>.
    /// </summary>
    public void Remove(IBindingInformation bindingInformation)
    {
        _list.Remove(bindingInformation);
    }

    /// <summary>
    /// Determines if a binding information is in List<T>.
    /// </summary>
    public bool Contains(string bindingInformation)
    {
        return _list.Find(binding
            => binding.BindingInformation == bindingInformation
            || binding.EndPoint == bindingInformation) != null;
    }

    /// <summary>
    /// Determines if a binding information is in List<T>.
    /// </summary>
    public bool Contains(IBindingInformation bindingInformation)
    {
        return _list.Contains(bindingInformation);
    }

    /// <summary>
    /// Searches for binding information that matches the condition defined by the specified predicate and returns the first matching element in the entire List<T>.
    /// </summary>
    public IBindingInformation? Find(Predicate<IBindingInformation> match)
    {
        return _list.Find(match);
    }

    /// <summary>
    /// Implementation iterator
    /// </summary>
    /// <returns></returns>
    public IEnumerator<IBindingInformation> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    /// <summary>
    /// Implementation iterator
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Override
    /// </summary>
    /// <param name="obj">Compare object</param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        return obj is BindingCollection collection &&
            _list.Intersect(collection._list).Any();
    }

    /// <summary>
    /// Override
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return 0;
    }
}
