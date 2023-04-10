#pragma warning disable CA1416 // 验证平台兼容性

using Driver.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Web.Administration;
using System.ServiceProcess;
using WSADM;
using WSADM.Interfaces;

namespace Driver;

public class IIS : IDriver
{
    public Result<IServerManager> GetServerManager(string? path)
    {
        // Dependency Injection（DI）
        // IoC Container
        IHost host = Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
            {
                services.AddSingleton<IServerManager, Manager>();
                services.AddSingleton<ServerManager>();
                services.AddSingleton(sp => new ServiceController("W3SVC"));
                services.AddSingleton<ISiteCollection<ISite>, Models.SiteCollection>();
            }).Build();

        return Result<IServerManager>.Ok(
            host.Services.GetRequiredService<Manager>()
            );
    }
}

#pragma warning restore CA1416 // 验证平台兼容性
