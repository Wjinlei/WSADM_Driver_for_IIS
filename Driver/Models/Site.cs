using WSADM.Interfaces;

namespace Driver.Models;

public class Site : ISite
{
    public string Name { get; set; }
    public string PhysicalPath { get; set; }
    public string RunPath { get; set; }
    public IBindingInformationCollection Bindings { get; set; }

    public Site(string name, string physicalPath, IBindingInformationCollection bindings)
    {
        Name = name;
        PhysicalPath = physicalPath;
        RunPath = physicalPath;
        Bindings = bindings;
    }

    public override bool Equals(object? obj)
    {
        return obj is Site site &&
               Name == site.Name &&
               EqualityComparer<IBindingInformationCollection>.Default.Equals(Bindings, site.Bindings);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Bindings);
    }
}
