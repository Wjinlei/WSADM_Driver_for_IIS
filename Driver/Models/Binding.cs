using WSADM.Interfaces;

namespace Driver.Models;

public class Binding : IBindingInformation
{
    public string Address { get; set; }
    public int Port { get; set; }
    public string Host { get; set; }

    public string EndPoint => Host + ":" + Port;
    public string BindingInformation
    {
        get
        {
            if (Address == "0.0.0.0")
                return "*" + ":" + Port + ":" + Host;
            return Address + ":" + Port + ":" + Host;
        }
    }


    public Binding(int port)
    {
        Address = "0.0.0.0";
        Port = port;
        Host = "";
    }

    public Binding(string domain, int port)
    {
        Address = "0.0.0.0";
        Port = port;
        Host = domain;
    }

    public Binding(string ipAddr, string domain, int port)
    {
        Address = ipAddr;
        Port = port;
        Host = domain;
    }

    public override string? ToString()
    {
        return BindingInformation;
    }

    public override bool Equals(object? obj)
    {
        return obj is Binding binding &&
               Address == binding.Address &&
               Port == binding.Port &&
               Host == binding.Host;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Address, Port, Host);
    }
}
