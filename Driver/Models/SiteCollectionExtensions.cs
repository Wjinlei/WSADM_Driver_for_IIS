using WSADM;
using WSADM.Interfaces;

namespace Driver.Models;

public static class SiteCollectionExtensions
{
    public static Result Check(this ISiteCollection<ISite> sites, ISite site)
    {
        var result = site.Check();
        if (result.Success)
            return result;

        if (sites.Contains(site))
            return Result.Error(new ArgumentException("This site already exists"));

        return Result.Ok;
    }
}
