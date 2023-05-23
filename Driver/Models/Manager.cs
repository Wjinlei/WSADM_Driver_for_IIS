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
        _sites = new SiteCollection(_serverManager);
    }

    public void CommitChanges()
    {
        _serverManager.Sites.Clear();

        // Add sites
        foreach (var _site in _sites)
        {
            // The first binding information for the site
            var _first = _site.Bindings.FirstOrDefault(new Binding("", 80));

            // Add sites using ServerManager
            var site = _serverManager.Sites.Add(
                _site.Name,
                "http",
                _first.BindingInformation,
                _site.PhysicalPath
                );

            // Continue adding the remaining binding information
            foreach (var binding in _site.Bindings)
            {
                if (binding.BindingInformation == _first.BindingInformation) continue;
                site.Bindings.Add(binding.BindingInformation, "http");
            }

            // Configuration site
            // Limits
            site.Limits.ConnectionTimeout = _site.Limits.ConnectionTimeout;
            site.Limits.MaxUrlSegments = _site.Limits.MaxUrlSegments;
            site.Limits.MaxConnections = _site.Limits.MaxConnections;
            site.Limits.MaxBandwidth = _site.Limits.MaxBandwidth;
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

#pragma warning restore CA1416 // ServiceController 只支持Windows，但这个dll本就是给windows用的，因此忽略这个警告
