using WSADM.Interfaces;

namespace Driver.Models;

public class BindingInformation : IBindingInformation
{
    public string DomainName { get; set; }
    public int Port { get; set; }
    public string IpAddr { get; set; }

    public string EndPoint => DomainName + ":" + Port;
    public string Host => IpAddr + ":" + Port + ":" + DomainName;

    public BindingInformation(int port)
    {
        DomainName = "";
        Port = port;
        IpAddr = "*";
    }

    public BindingInformation(string domain, int port)
    {
        DomainName = domain;
        Port = port;
        IpAddr = "*";
    }

    public BindingInformation(string ipAddr, string domain, int port)
    {
        DomainName = domain;
        Port = port;
        IpAddr = ipAddr;
    }

    public override bool Equals(object? obj)
    {
        return obj is BindingInformation information &&
               DomainName == information.DomainName &&
               Port == information.Port &&
               IpAddr == information.IpAddr;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(DomainName, Port);
    }
}
