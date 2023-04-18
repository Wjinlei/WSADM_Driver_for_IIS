using WSADM;
using WSADM.Interfaces;

namespace Driver.Models;

public static class SiteExtensions
{
    public static Result Check(this ISite site)
    {
        if (site.Bindings.Count < 1)
            return Result.Error(new ArgumentException("The binding information cannot be empty"));

        if (string.IsNullOrWhiteSpace(site.Name))
            return Result.Error(new ArgumentException("The site name cannot be empty"));

        if (string.IsNullOrWhiteSpace(site.PhysicalPath))
            return Result.Error(new ArgumentException("The physical path cannot be empty"));

        return Result.Ok;
    }
}
