using Microsoft.Web.Administration;
using WSADM;

namespace Driver.Models;

public static class SiteCollectionExtensions
{
    /// <summary>
    /// It's just a wrapper around the Add method that catches the exception
    /// </summary>
    /// <param name="siteCollection"></param>
    /// <param name="site"></param>
    /// <returns></returns>
    public static Result TryAdd(
        this Microsoft.Web.Administration.SiteCollection siteCollection,
        Microsoft.Web.Administration.Site site)
    {
        try
        {
            siteCollection.Add(site);
        }
        catch (Exception ex)
        {
            return Result.Error(ex);
        }

        return Result.Ok;
    }

    /// <summary>
    /// Create a site object
    /// </summary>
    /// <param name="siteCollection"></param>
    /// <param name="name"></param>
    /// <param name="physicalPath"></param>
    /// <returns></returns>
    public static Microsoft.Web.Administration.Site CreateSiteDefaults(
        this Microsoft.Web.Administration.SiteCollection siteCollection,
        string name,
        string physicalPath)
    {
        var _site = siteCollection.CreateElement();

        // Base settings
        _site.Id = siteCollection.Count + 1;
        _site.Name = name;

        // Add app
        Application app = _site.Applications.CreateElement();
        app.Path = "/";
        app.VirtualDirectories.Add("/", physicalPath);
        _site.Applications.Add(app);

        return _site;
    }

    /// <summary>
    /// Find a website by its name
    /// </summary>
    /// <param name="siteCollection"></param>
    /// <param name="siteName"></param>
    /// <returns></returns>
    public static Microsoft.Web.Administration.Site? FirstOrDefaultEx(
        this Microsoft.Web.Administration.SiteCollection siteCollection,
        string siteName)
    {
        return siteCollection.FirstOrDefault(site => site.Name == siteName);
    }
}
