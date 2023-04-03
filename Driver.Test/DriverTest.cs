using NUnit.Framework.Interfaces;
using WSADM.Interfaces;

namespace Driver.Test;

public class Tests
{
    [Test]
    public void TestDriver()
    {
        // Get Driver
        var driverResult = WSADM.DriverManager.RegisterDriver("Driver.IIS", "Driver.dll");
        Assert.That(driverResult.Success, Is.True);
        var driver = driverResult.GetOk();

        // Get Server
        var serverResult = driver.GetServerManager(null);
        Assert.That(serverResult.Success, Is.True);
        var iis = serverResult.GetOk();

        // Add Site example
        iis.Sites.Add("www.test1.com", "d:/wwwroot", "www.test1.com", 80);
        iis.Sites["www.test1.com"]?.Bindings.Add("bbs.test1.com", 8080);
        iis.Sites["www.test1.com"]?.Bindings.Add("m.test1.com", 80);

        // Delete site example
        //var test1 = iis.Sites["www.test1.com"];
        //Assert.That(test1, Is.Not.Null);
        //iis.Sites.Remove(test1);
        //iis.Sites.Remove("www.test2.com"); // It can also be removed by passing the website name

        // Modify site binding information
        var binding = iis.Sites["www.test1.com"]?.Bindings["*:80:m.test1.com"];
        Assert.That(binding, Is.Not.Null);
        binding.Port = 8080;

        // Delete binding information
        iis.Sites["www.test1.com"]?.Bindings.Remove("*:8080:bbs.test1.com");

        // Commit
        iis.CommitChanges();
    }
}
