#pragma warning disable CA1416 // ServiceController 只支持Windows，但这个dll本就是给windows用的，因此忽略这个警告

using Microsoft.Web.Administration;
using System.ServiceProcess;
using WSADM.Interfaces;

namespace Driver.Models;

public class Manager : IServerManager
{
    private readonly ServiceController _serviceController;
    private readonly ServerManager _serverManager;
    private readonly ISiteCollection<ISite> _sites;

    public ISiteCollection<ISite> Sites => _sites;

    public Manager()
    {
        _serviceController = new ServiceController("W3SVC");
        _serverManager = new ServerManager();
        _sites = new SiteCollection(_serverManager.Sites);
    }

    public void CommitChanges()
    {
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

#pragma warning restore CA1416 // ServiceController 只支持Windows，但这个dll本就是给windows用的，因此忽略这个警告
