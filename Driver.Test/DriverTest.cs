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
        //_serverManager.Sites.Add("www.test1.com", "d:/wwwroot", 80);
        //_serverManager.Sites["www.test1.com"]?.Bindings.Add("www.test1.com", 80);
        //_serverManager.Sites["www.test1.com"]?.Bindings.Add("bbs.test1.com", 8080);
        //_serverManager.Sites["www.test1.com"]?.Bindings.Add("m.test1.com", 80);

        _serverManager.Sites.Add("www.test1.com", "d:/wwwroot", new List<string>
        {
            "www.test1.com",
            "bbs.test1.com:8080",
            "m.test1.com:80"
        });
    }

    [Test]
    public void TestSiteDelete()
    {
        _serverManager.Sites.Remove("www.test1.com");

        //var test1 = _serverManager.Sites["www.test1.com"];
        //Assert.That(test1, Is.Not.Null);
        //_serverManager.Sites.Remove(test1);
    }

    [Test]
    public void TestSiteModify()
    {
        // Get site
        var site = _serverManager.Sites["www.test1.com"];
        Assert.That(site, Is.Not.Null);

        // Modify binding
        var bind = site.Bindings["m.test1.com:80"];
        Assert.That(bind, Is.Not.Null);
        bind.Port = 8080; // Modify port

        site.Bindings.Add("new.test1.com:8088"); // Add binding information
        site.Bindings.Remove("bbs.test1.com:8080"); // Delete binding information
    }

    [TearDown]
    public void Commit()
    {
        _serverManager.CommitChanges();
    }
}
