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
            var svc = new SerilogLogService();
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

    [Fact]
    public async Task LogError_WritesFile()
    {
        var tempHome = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempHome);
        var oldAppData = Environment.GetEnvironmentVariable("APPDATA");
        Environment.SetEnvironmentVariable("APPDATA", tempHome);

        try
        {
            var svc = new SerilogLogService();
            await svc.LogError("test", new InvalidOperationException());

            var logDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Wrecept", "logs");
            Assert.True(Directory.Exists(logDir));
            var files = Directory.GetFiles(logDir);
            Assert.NotEmpty(files);
            var content = await File.ReadAllTextAsync(files[0]);
            Assert.Contains("test", content);
        }
        finally
        {
            Environment.SetEnvironmentVariable("APPDATA", oldAppData);
            if (Directory.Exists(tempHome))
            {
                Directory.Delete(tempHome, recursive: true);
            }
        }
    }
}
