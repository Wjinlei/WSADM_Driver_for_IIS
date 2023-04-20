using WSADM;
using WSADM.Interfaces;
using System.Collections;

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
    /// Todo: This function always feels a little verbose
    /// </summary>
    /// <param name="bindingInformation"></param>
    /// <returns></returns>
    public Result Add(string bindingInformation)
    {
        var binding = bindingInformation.Split(":");
        if (binding.Length < 3)
        {
            // Example: www.example.com:8080
            var domainNameOrDefault = binding.ElementAtOrDefault(0) ?? "";
            var portOrDefault = binding.ElementAtOrDefault(1) ?? "80";
            return Add(new Binding(
                domainNameOrDefault,
                Mojito.Convert.ToInt32OrDefault(portOrDefault, 80)));
        }
        else
        {
            // Example: 127.0.0.1:8080:www.example.com
            var ipAddrOrDefault = binding.ElementAtOrDefault(0) ?? "*";
            var portOrDefault = binding.ElementAtOrDefault(1) ?? "80";
            var domainNameOrDefault = binding.ElementAtOrDefault(2) ?? "";
            return Add(new Binding(
                ipAddrOrDefault,
                domainNameOrDefault,
                Mojito.Convert.ToInt32OrDefault(portOrDefault, 80)));
        }
    }

    // Implementation List
    public void Clear()
    {
        _list.Clear();
    }

    public void Remove(string bindingInformation)
    {
        var binding = _list.Find(binding => binding.BindingInformation == bindingInformation || binding.EndPoint == bindingInformation);
        if (binding != null)
            _ = _list.Remove(binding);
    }

    public void Remove(IBindingInformation bindingInformation)
    {
        _list.Remove(bindingInformation);
    }

    public bool Contains(string bindingInformation)
    {
        return _list.Find(binding => binding.BindingInformation == bindingInformation || binding.EndPoint == bindingInformation) != null;
    }

    public bool Contains(IBindingInformation bindingInformation)
    {
        return _list.Contains(bindingInformation);
    }

    public IBindingInformation? Find(Predicate<IBindingInformation> match)
    {
        return _list.Find(match);
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
        return obj is BindingCollection collection &&
            _list.Intersect(collection._list).Any();
    }

    public override int GetHashCode()
    {
        return 0;
    }
}
