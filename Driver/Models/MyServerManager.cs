using Microsoft.Web.Administration;
using WSADM.Interfaces;

namespace Driver.Models;

public class MyServerManager : IServerManager
{
    private readonly ServerManager _serverManager;
    private readonly SiteCollection _sites;

    public ISiteCollection<ISite> Sites => _sites;

    public MyServerManager()
    {
        _serverManager = new ServerManager();
        _sites = new SiteCollection(_serverManager);
    }

    public void CommitChanges()
    {
        _serverManager.Sites.Clear();

        foreach (var site in _sites)
        {
            var bindingDefault = "*:80:";
            var first = site.Bindings.FirstOrDefault();
            if (first != null)
                bindingDefault = first.ToString();

            var siteServerManager = _serverManager.Sites.Add(site.Name, "http", bindingDefault, site.PhysicalPath);
            foreach (var binding in site.Bindings)
            {
                if (binding.ToString() == bindingDefault) continue;
                siteServerManager.Bindings.Add(binding.ToString(), "http");
            }
        }

        _serverManager.CommitChanges();
    }

    public void Reload()
    {
        throw new NotImplementedException();
    }

    public void Restart()
    {
        throw new NotImplementedException();
    }

    public void Start()
    {
        throw new NotImplementedException();
    }

    public void Stop()
    {
        throw new NotImplementedException();
    }
}
