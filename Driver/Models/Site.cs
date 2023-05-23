using WSADM.Interfaces;

namespace Driver.Models;

public class Site : ISite
{
    public string Name { get; set; }
    public string PhysicalPath { get; set; }
    public string RunPath { get; set; }
    public IBindingInformationCollection Bindings { get; set; }
    public ISiteLimits Limits { get; set; }

    public Site(string name, string physicalPath, IBindingInformationCollection bindings)
    {
        Name = name;
        PhysicalPath = physicalPath;
        RunPath = physicalPath;
        Bindings = bindings;
        Limits = new SiteLimits(TimeSpan.FromSeconds(120), 32, 4294967295, 4294967295);
    }

    /// <summary>
    /// Override
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        return obj is Site site && Name == site.Name
            && EqualityComparer<IBindingInformationCollection>.Default.Equals(Bindings, site.Bindings);
    }

    /// <summary>
    /// Override
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Name);
    }
}
