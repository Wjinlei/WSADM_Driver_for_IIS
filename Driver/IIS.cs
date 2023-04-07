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
        IHost host = Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
                services.AddSingleton<Manager>()
                        .AddSingleton<ServerManager>()
                        .AddSingleton(sp => new ServiceController("W3SVC"))
                        .AddSingleton<ISiteCollection<ISite>, Models.SiteCollection>())
            .Build();

        var serverManager = host.Services.GetRequiredService<Manager>();
        return Result<IServerManager>.Ok(serverManager);
    }
}

#pragma warning restore CA1416 // 验证平台兼容性
