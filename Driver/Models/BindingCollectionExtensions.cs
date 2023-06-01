using WSADM;
using WSADM.Interfaces;

namespace Driver.Models;

public static class BindingCollectionExtensions
{
    /// <summary>
    /// It's just a wrapper around the Add method that catches the exception
    /// </summary>
    /// <param name="bindingCollection"></param>
    /// <param name="binding"></param>
    /// <returns></returns>
    public static Result TryAdd(
        this Microsoft.Web.Administration.BindingCollection bindingCollection,
        Microsoft.Web.Administration.Binding binding)
    {
        try
        {
            bindingCollection.Add(binding);
        }
        catch (Exception ex)
        {
            return Result.Error(ex);
        }

        return Result.Ok;
    }

    /// <summary>
    /// Parse the binding string and add it
    /// </summary>
    /// <param name="bindingCollection"></param>
    /// <param name="bindingInformationStr"></param>
    /// <returns></returns>
    public static Result TryAdd(
        this Microsoft.Web.Administration.BindingCollection bindingCollection,
        string bindingInformationStr)
    {
        var array = bindingInformationStr.Split(":");
        var ipAddrOrDefault = array.ElementAtOrDefault(array.Length > 2 ? 0 : -1) ?? "*";
        var domainNameOrDefault = array.ElementAtOrDefault(array.Length > 2 ? 2 : 0) ?? "";
        var portOrDefault = array.ElementAtOrDefault(1) ?? "80";

        var binding = CreateBindingDefaults(bindingCollection,
                                            ipAddrOrDefault,
                                            domainNameOrDefault,
                                            Mojito.Convert.ToInt32OrDefault(portOrDefault, 80));

        return TryAdd(bindingCollection, binding);
    }

    /// <summary>
    /// Overload TryAdd
    /// </summary>
    /// <param name="bindingCollection"></param>
    /// <param name="binding"></param>
    /// <returns></returns>
    public static Result TryAdd(
        this Microsoft.Web.Administration.BindingCollection bindingCollection,
        IBindingInformation binding)
    {
        return TryAdd(bindingCollection, binding.BindingInformation);
    }

    /// <summary>
    /// Adds a list of binding strings
    /// </summary>
    /// <param name="bindingCollection"></param>
    /// <param name="bindings"></param>
    /// <returns></returns>
    public static Result TryAddCollection(
        this Microsoft.Web.Administration.BindingCollection bindingCollection,
        List<string> bindings)
    {
        foreach (var binding in bindings)
        {
            var result = bindingCollection.TryAdd(binding);
            if (!result.Success) return result;
        }

        return Result.Ok;
    }

    /// <summary>
    /// Overload TryAddCollection
    /// </summary>
    /// <param name="bindingCollection"></param>
    /// <param name="bindings"></param>
    /// <returns></returns>
    public static Result TryAddCollection(
        this Microsoft.Web.Administration.BindingCollection bindingCollection,
        IBindingInformationCollection bindings)
    {
        return TryAddCollection(bindingCollection,
            bindings.Select(binding => binding.BindingInformation).ToList());
    }

    /// <summary>
    /// Create a binding information object
    /// </summary>
    /// <param name="bindingCollection"></param>
    /// <param name="ipAddr"></param>
    /// <param name="domain"></param>
    /// <param name="port"></param>
    /// <returns></returns>
    public static Microsoft.Web.Administration.Binding CreateBindingDefaults(
        this Microsoft.Web.Administration.BindingCollection bindingCollection,
        string ipAddr,
        string domain,
        int port)
    {
        var binding = bindingCollection.CreateElement();
        binding.Protocol = "http";
        binding.BindingInformation = ipAddr + ":" + port + ":" + domain;
        return binding;
    }

    /// <summary>
    /// Overload CreateBindingDefaults
    /// </summary>
    /// <param name="bindingCollection"></param>
    /// <param name="domain"></param>
    /// <param name="port"></param>
    /// <returns></returns>
    public static Microsoft.Web.Administration.Binding CreateBindingDefaults(
        this Microsoft.Web.Administration.BindingCollection bindingCollection,
        string domain,
        int port)
    {
        return CreateBindingDefaults(bindingCollection, "*", domain, port);
    }

    /// <summary>
    /// Finds the specified binding information
    /// </summary>
    /// <param name="bindingCollection"></param>
    /// <param name="bindingInformationStr"></param>
    /// <returns></returns>
    public static Microsoft.Web.Administration.Binding? FirstOrDefaultEx(
        this Microsoft.Web.Administration.BindingCollection bindingCollection,
        string bindingInformationStr)
    {
        return bindingCollection.FirstOrDefault(binding
            => binding.BindingInformation == bindingInformationStr
            || binding.Host + ":" + binding.EndPoint.Port == bindingInformationStr);
    }
}
