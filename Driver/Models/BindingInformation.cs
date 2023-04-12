using WSADM.Interfaces;

namespace Driver.Models;

public class BindingInformation : IBindingInformation
{
    public string IpAddr { get; set; }
    public int Port { get; set; }
    public string Domain { get; set; }

    public BindingInformation(int port)
    {
        IpAddr = "*";
        Port = port;
        Domain = "";
    }

    public BindingInformation(string domain, int port)
    {
        IpAddr = "*";
        Port = port;
        Domain = domain;
    }

    public BindingInformation(string ipAddr, string domain, int port)
    {
        IpAddr = ipAddr;
        Port = port;
        Domain = domain;
    }

    public string ToBind()
    {
        return IpAddr + ":" + Port + ":" + Domain;
    }

    public string ToUrl()
    {
        return Domain + ":" + Port;
    }

    public override bool Equals(object? obj)
    {
        return obj is BindingInformation information &&
               IpAddr == information.IpAddr &&
               Port == information.Port &&
               Domain == information.Domain;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Domain, Port);
    }
}
