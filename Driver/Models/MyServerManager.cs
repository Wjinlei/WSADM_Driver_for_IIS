#pragma warning disable CA1416 // 验证平台兼容性，由于这本来就是IIS用的Dll，因此禁用这个检查

using Microsoft.Web.Administration;
using System.ServiceProcess;
using WSADM.Interfaces;

namespace Driver.Models;

public class MyServerManager : IServerManager
{
    private readonly ServerManager _serverManager;
    private readonly SiteCollection _sites;
    private readonly ServiceController _serviceController;

    public ISiteCollection<ISite> Sites => _sites;

    public MyServerManager()
    {
        _serviceController = new ServiceController("W3SVC");
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
        _serviceController.Refresh();
    }

    public void Restart()
    {
        _serviceController.Stop();
        _serviceController.Start();
    }

    public void Start()
    {
        _serviceController.Start();
    }

    public void Stop()
    {
        _serviceController.Stop();
    }
}

#pragma warning restore CA1416 // 验证平台兼容性，由于这本来就是IIS用的Dll，因此禁用这个检查
