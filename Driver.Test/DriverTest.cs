using WSADM.Interfaces;

namespace Driver.Test;

public class Tests
{
    private IServerManager _serverManager;

    [SetUp]
    public void Setup()
    {
        // Get Driver
        var driverResult = WSADM.DriverManager.RegisterDriver("Driver.IIS", "Driver.dll");
        Assert.That(driverResult.Success, Is.True);
        var driver = driverResult.GetOk();

        // Get Server
        var serverResult = driver.GetServerManager(null);
        Assert.That(serverResult.Success, Is.True);
        _serverManager = serverResult.GetOk();
    }


    [Test]
    public void TestAdd()
    {
        var result = _serverManager.Sites.Add("test1.example.com", "d:/wwwroot", new List<string>
        {
            "test1.example.com",
            "bbs1.example.com",
            "127.0.0.1:8080:dev1.example.com" // Bind to the specified IP address
        });
        if (!result.Success)
            TestContext.Out.WriteLine(result.Message);

        Assert.That(result.Success, Is.True);
    }

    [Test]
    public void TestRemove()
    {
        var result = _serverManager.Sites.Add("test2.example.com", "d:/wwwroot", "test2.example.com", 80);
        Assert.That(result.Success, Is.True);

        _serverManager.Sites.Remove("test2.example.com"); // Remove

        var site = _serverManager.Sites["test2.example.com"];
        Assert.That(site, Is.Null);
    }

    [Test]
    public void TestModifyBindings()
    {

        var result = _serverManager.Sites.Add("test3.example.com", "d:/wwwroot", "test3.example.com", 80);
        Assert.That(result.Success, Is.True);
        var site = _serverManager.Sites["test3.example.com"];
        Assert.That(site, Is.Not.Null);

        site.Bindings.Add("bbs3.example.com");
        site.Bindings.Add("127.0.0.1:8080:dev3.example.com");

        // Modify binding
        var bind = site.Bindings["bbs3.example.com:80"];
        Assert.That(bind, Is.Not.Null);

        var r = site.Bindings.Add(bind.Host, 8080);
        Assert.That(r.Success, Is.True);
        site.Bindings.Remove(bind);
    }

    [Test]
    public void TestModifyLimits()
    {
        var result = _serverManager.Sites.Add("test4.example.com", "d:/wwwroot", "test4.example.com", 80);
        Assert.That(result.Success, Is.True);
        var site = _serverManager.Sites["test4.example.com"];
        Assert.That(site, Is.Not.Null);

        site.Limits.ConnectionTimeout = TimeSpan.FromSeconds(300);
        site.Limits.MaxUrlSegments = 64;
        site.Limits.MaxBandwidth = 102400;
        site.Limits.MaxConnections = 600;
    }

    [TearDown]
    public void Commit()
    {
        _serverManager.CommitChanges();
    }
}
