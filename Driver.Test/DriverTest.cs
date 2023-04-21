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
    public void TestSiteAdd()
    {
        //_serverManager.Sites.Add("www.example.com", "d:/wwwroot", 80);
        //_serverManager.Sites["www.example.com"]?.Bindings.Add("www.example.com", 80);
        //_serverManager.Sites["www.example.com"]?.Bindings.Add("bbs.example.com", 8080);
        //_serverManager.Sites["www.example.com"]?.Bindings.Add("m.example.com", 80);

        var result = _serverManager.Sites.Add("www.example.com", "d:/wwwroot", new List<string>
        {
            "www.example.com",
            "bbs.example.com:8080",
            "m.example.com:80",
            "127.0.0.1:8080:dev.example.com" // Bind to the specified IP address
        });
        if (!result.Success)
            TestContext.Out.WriteLine(result.Message);
        Assert.That(result.Success, Is.True);
    }

    [Test]
    public void TestSiteDelete()
    {
        _serverManager.Sites.Remove("www.example.com");

        //var example = _serverManager.Sites["www.example.com"];
        //Assert.That(example, Is.Not.Null);
        //_serverManager.Sites.Remove(example);
    }

    [Test]
    public void TestSiteModify()
    {
        // Get site
        var example = _serverManager.Sites["www.example.com"];
        Assert.That(example, Is.Not.Null);

        // Modify binding
        var bind = example.Bindings["m.example.com:80"];
        Assert.That(bind, Is.Not.Null);
        bind.Port = 8080; // Modify port

        example.Bindings.Add("127.0.0.1:9999:test.example.com"); // Add binding information
        example.Bindings.Add("new.example.com:8088"); // Add binding information
        example.Bindings.Remove("bbs.example.com:8080"); // Delete binding information
    }

    [TearDown]
    public void Commit()
    {
        _serverManager.CommitChanges();
    }
}
