using System;
using System.IO;
using System.Threading.Tasks;
using Wrecept.Storage.Services;
using Xunit;

namespace Wrecept.Storage.Tests;

public class LogServiceTests
{
    [Fact]
    public async Task LogError_FallsBackToConsole()
    {
        var tempHome = "/proc";
        var oldAppData = Environment.GetEnvironmentVariable("APPDATA");
        Environment.SetEnvironmentVariable("APPDATA", tempHome);
        await using var sw = new StringWriter();
        var oldErr = Console.Error;
        Console.SetError(sw);
        try
        {
            var svc = new LogService();
            await svc.LogError("err", new InvalidOperationException());
            var logDir = Path.Combine(tempHome, "Wrecept", "logs");
            Assert.False(Directory.Exists(logDir));
            Assert.Contains("err", sw.ToString());
        }
        finally
        {
            Console.SetError(oldErr);
            Environment.SetEnvironmentVariable("APPDATA", oldAppData);
        }
    }
}
