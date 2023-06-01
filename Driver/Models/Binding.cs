using WSADM.Interfaces;
using System.Net;

namespace Driver.Models;

public class Binding : IBindingInformation
{
    private readonly Microsoft.Web.Administration.Binding _bindingInformation;

    public IPAddress Address
    {
        get
        {
            return _bindingInformation.EndPoint.Address;
        }
    }

    public string Host
    {
        get
        {
            return _bindingInformation.Host;
        }
    }

    public int Port
    {
        get
        {
            return _bindingInformation.EndPoint.Port;
        }
    }

    public string BindingInformation
    {
        get
        {
            return _bindingInformation.BindingInformation;
        }
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="bindingInformation"></param>
    public Binding(Microsoft.Web.Administration.Binding bindingInformation)
    {
        _bindingInformation = bindingInformation;
    }

    /// <summary>
    /// Override
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return BindingInformation;
    }

    /// <summary>
    /// Override
    /// </summary>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        return _bindingInformation.Equals(obj);
    }

    /// <summary>
    /// Override
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return _bindingInformation.GetHashCode();
    }
}
