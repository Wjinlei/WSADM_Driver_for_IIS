using WSADM.Interfaces;

namespace Driver.Models;

public class BindingInformation : IBindingInformation
{
    public string Domain { get; set; }
    public int Port { get; set; }

    public BindingInformation(string domain, int port)
    {
        Domain = domain;
        Port = port;
    }

    public override string ToString()
    {
        return "*:" + Port + ":" + Domain;
    }

    public override bool Equals(object? obj)
    {
        return obj is BindingInformation information &&
               Domain == information.Domain &&
               Port == information.Port;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Domain, Port);
    }
}
