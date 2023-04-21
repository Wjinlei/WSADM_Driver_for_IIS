using Driver.Models;
using WSADM;
using WSADM.Interfaces;

namespace Driver;

public class IIS : IDriver
{
    /// <summary>
    /// Gets the implementation class for IServerManager
    /// </summary>
    /// <param name="path">The WebServer installation path, or null if not required</param>
    /// <returns></returns>
    public Result<IServerManager> GetServerManager(string? path)
    {
        return Result<IServerManager>.Ok(new Manager());
    }
}
