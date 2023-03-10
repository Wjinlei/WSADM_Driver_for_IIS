using Driver.Models;
using WSADM;
using WSADM.Interfaces;

namespace Driver;

public class IIS : IDriver
{
    public Result<IServerManager> GetServerManager(string? path)
    {
        return Result<IServerManager>.Ok(new MyServerManager());
    }
}
