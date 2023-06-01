using System.Collections;
using WSADM;
using WSADM.Interfaces;

namespace Driver.Models;

public class BindingCollection : IBindingInformationCollection
{
    // Private
    private readonly Microsoft.Web.Administration.BindingCollection _bindingCollection;

    // Public
    public int Count => _bindingCollection.Count;

    // Indexer
    public IBindingInformation this[int index]
    {
        get
        {
            return new Binding(_bindingCollection[index]);
        }
    }

    public IBindingInformation? this[string bindingInformation]
    {
        get
        {
            var binding = _bindingCollection.FirstOrDefaultEx(bindingInformation);
            return binding != null ? new Binding(binding) : null;
        }
    }

    // Constructor
    public BindingCollection(Microsoft.Web.Administration.BindingCollection bindingCollection)
    {
        _bindingCollection = bindingCollection;
    }

    // Methods
    // Check parameter
    public Result Add(string bindingInformation)
    {
        return _bindingCollection.TryAdd(bindingInformation);
    }

    public Result Add(IBindingInformation bindingInformation)
    {
        return _bindingCollection.TryAdd(bindingInformation);
    }

    public Result Add(string domain, int port)
    {
        var binding = _bindingCollection.CreateBindingDefaults(domain, port);
        return _bindingCollection.TryAdd(binding);
    }

    public Result Add(string ipAddr, string domain, int port)
    {
        var binding = _bindingCollection.CreateBindingDefaults(ipAddr, domain, port);
        return _bindingCollection.TryAdd(binding);
    }

    /// <summary>
    /// Removes all elements from List<T>.
    /// </summary>
    public void Clear()
    {
        _bindingCollection.Clear();
    }

    /// <summary>
    /// Find and Removes the first match of a particular object from List<T>.
    /// </summary>
    public void Remove(string bindingInformation)
    {
        var binding = _bindingCollection.FirstOrDefaultEx(bindingInformation);
        if (binding != null)
            _bindingCollection.Remove(binding);
    }

    /// <summary>
    /// Removes the first match of a particular object from List<T>.
    /// </summary>
    public void Remove(IBindingInformation bindingInformation)
    {
        Remove(bindingInformation.BindingInformation);
    }

    /// <summary>
    /// Determines if a binding information is in List<T>.
    /// </summary>
    public bool Contains(string bindingInformation)
    {
        return _bindingCollection.FirstOrDefaultEx(bindingInformation) != null;
    }

    /// <summary>
    /// Determines if a binding information is in List<T>.
    /// </summary>
    public bool Contains(IBindingInformation bindingInformation)
    {
        return Contains(bindingInformation.BindingInformation);
    }

    /// <summary>
    /// Implementation iterator
    /// </summary>
    /// <returns></returns>
    public IEnumerator<IBindingInformation> GetEnumerator()
    {
        return _bindingCollection.Select(binding => new Binding(binding)).ToList().GetEnumerator();
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
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        return _bindingCollection.Equals(obj);
    }

    /// <summary>
    /// Override
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return _bindingCollection.GetHashCode();
    }
}
