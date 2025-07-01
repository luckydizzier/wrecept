using System;
using System.IO;
using System.Threading.Tasks;
using Wrecept.Core.Entities;
using Wrecept.Core.Services;
using Wrecept.Storage.Services;
using Xunit;

namespace Wrecept.Core.Tests.Services;

public class UserInfoServiceTests : IDisposable
{
    private readonly string _tempDir;

    public UserInfoServiceTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_tempDir);
        Environment.SetEnvironmentVariable("XDG_CONFIG_HOME", _tempDir);
        Environment.SetEnvironmentVariable("APPDATA", _tempDir);
    }

    [Fact]
    public async Task LoadAsync_ReturnsDefaults_IfFileMissing()
    {
        var svc = new UserInfoService();
        var info = await svc.LoadAsync();

        Assert.Equal(string.Empty, info.CompanyName);
        Assert.Equal(string.Empty, info.Address);
        Assert.Equal(string.Empty, info.Phone);
        Assert.Equal(string.Empty, info.Email);
    }

    [Fact]
    public async Task SaveAsync_WritesFile()
    {
        var svc = new UserInfoService();
        var info = new UserInfo { CompanyName = "ACME" };
        await svc.SaveAsync(info);

        var path = Path.Combine(_tempDir, "Wrecept", "wrecept.json");
        Assert.True(File.Exists(path));
    }

    [Fact]
    public async Task LoadAsync_ReturnsSavedObject()
    {
        var svc = new UserInfoService();
        var info = new UserInfo { CompanyName = "ACME", Address = "Addr", Phone = "123", Email = "a@b.c" };
        await svc.SaveAsync(info);

        var loaded = await svc.LoadAsync();
        Assert.Equal(info.CompanyName, loaded.CompanyName);
        Assert.Equal(info.Address, loaded.Address);
        Assert.Equal(info.Phone, loaded.Phone);
        Assert.Equal(info.Email, loaded.Email);
    }

    public void Dispose()
    {
        Environment.SetEnvironmentVariable("XDG_CONFIG_HOME", null);
        Environment.SetEnvironmentVariable("APPDATA", null);
        if (Directory.Exists(_tempDir))
            Directory.Delete(_tempDir, true);
    }
}
