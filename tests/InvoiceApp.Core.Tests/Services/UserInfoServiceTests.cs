using System;
using System.IO;
using System.Threading.Tasks;
using InvoiceApp.Core.Entities;
using InvoiceApp.Core.Services;
using InvoiceApp.Data.Services;
using Xunit;

namespace InvoiceApp.Core.Tests.Services;

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
        var path = Path.Combine(_tempDir, "InvoiceApp", "invoiceapp.json");
        var svc = new UserInfoService(path);
        var info = await svc.LoadAsync();

        Assert.Equal(string.Empty, info.CompanyName);
        Assert.Equal(string.Empty, info.Address);
        Assert.Equal(string.Empty, info.Phone);
        Assert.Equal(string.Empty, info.Email);
        Assert.Equal(string.Empty, info.TaxNumber);
        Assert.Equal(string.Empty, info.BankAccount);
    }

    [Fact]
    public async Task SaveAsync_WritesFile()
    {
        var path = Path.Combine(_tempDir, "InvoiceApp", "invoiceapp.json");
        var svc = new UserInfoService(path);
        var info = new UserInfo
        {
            CompanyName = "ACME",
            Address = "Addr",
            Phone = "123",
            Email = "a@b.c",
            TaxNumber = "12345678-1-42",
            BankAccount = "11100000-11111111-11111111"
        };
        await svc.SaveAsync(info);

        Assert.True(File.Exists(path));
    }

    [Fact]
    public async Task LoadAsync_ReturnsSavedObject()
    {
        var path = Path.Combine(_tempDir, "InvoiceApp", "invoiceapp.json");
        var svc = new UserInfoService(path);
        var info = new UserInfo
        {
            CompanyName = "ACME",
            Address = "Addr",
            Phone = "123",
            Email = "a@b.c",
            TaxNumber = "12345678-1-42",
            BankAccount = "11100000-11111111-11111111"
        };
        await svc.SaveAsync(info);

        var loaded = await svc.LoadAsync();
        Assert.Equal(info.CompanyName, loaded.CompanyName);
        Assert.Equal(info.Address, loaded.Address);
        Assert.Equal(info.Phone, loaded.Phone);
        Assert.Equal(info.Email, loaded.Email);
        Assert.Equal(info.TaxNumber, loaded.TaxNumber);
        Assert.Equal(info.BankAccount, loaded.BankAccount);
    }

    public void Dispose()
    {
        Environment.SetEnvironmentVariable("XDG_CONFIG_HOME", null);
        Environment.SetEnvironmentVariable("APPDATA", null);
        if (Directory.Exists(_tempDir))
            Directory.Delete(_tempDir, true);
    }
}
