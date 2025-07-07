using System;
using System.IO;
using System.Threading.Tasks;
using Wrecept.Core.Entities;
using Wrecept.Storage.Services;
using Xunit;

namespace Wrecept.Storage.Tests;

public class UserInfoServiceTests
{
    [Fact]
    public async Task LoadAsync_ReturnsEmpty_WhenFileMissing()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "u.json");
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
    public async Task SaveAndLoad_RoundTrip()
    {
        var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(dir);
        var path = Path.Combine(dir, "user.json");
        var svc = new UserInfoService(path);
        var info = new UserInfo
        {
            CompanyName = "ACME",
            Address = "Addr",
            Phone = "123",
            Email = "a@b.c",
            TaxNumber = "1",
            BankAccount = "2"
        };
        await svc.SaveAsync(info);

        var loaded = await svc.LoadAsync();

        Assert.Equal(info.CompanyName, loaded.CompanyName);
        Assert.Equal(info.Address, loaded.Address);
        Assert.Equal(info.Phone, loaded.Phone);
        Assert.Equal(info.Email, loaded.Email);
        Assert.Equal(info.TaxNumber, loaded.TaxNumber);
        Assert.Equal(info.BankAccount, loaded.BankAccount);

        Directory.Delete(dir, true);
    }
}
