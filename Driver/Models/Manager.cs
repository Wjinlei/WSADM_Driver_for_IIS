#pragma warning disable CA1416 // 验证平台兼容性，由于这本来就是IIS用的Dll，因此禁用这个检查

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

    // Dependency Injection（DI）
    public Manager(
        ServerManager serverManager,
        ServiceController serviceController,
        ISiteCollection<ISite> sites)
    {
        _serviceController = serviceController;
        _serverManager = serverManager;
        _sites = sites;
    }

    public void CommitChanges()
    {
        _serverManager.Sites.Clear();

        foreach (var _site in _sites)
        {
            var bindingInformation = "*:80:";
            var firstBinding = _site.Bindings.FirstOrDefault();
            if (firstBinding != null)
                bindingInformation = firstBinding.ToUrl();

            var site = _serverManager.Sites.Add(_site.Name, "http", bindingInformation, _site.PhysicalPath);
            foreach (var binding in _site.Bindings)
            {
                if (binding.ToUrl() == bindingInformation) continue;
                site.Bindings.Add(binding.ToUrl(), "http");
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
