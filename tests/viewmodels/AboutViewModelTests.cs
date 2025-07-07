using System;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using Wrecept.Wpf.ViewModels;
using Wrecept.Core.Entities;
using Wrecept.Core.Services;

namespace Wrecept.Tests.ViewModels;

public class AboutViewModelTests
{
    private class FakeService : IUserInfoService
    {
        public Task<UserInfo> LoadAsync() => Task.FromResult(new UserInfo
        {
            CompanyName = "ACME",
            Address = "X",
            Phone = "1",
            Email = "a@b.c"
        });
        public Task SaveAsync(UserInfo info) => Task.CompletedTask;
    }

    [Fact]
    public void StripFrontMatter_RemovesHeader()
    {
        var mi = typeof(AboutViewModel).GetMethod("StripFrontMatter", BindingFlags.NonPublic | BindingFlags.Static)!;
        var text = "---\ntitle: t\n---\nHello";
        var result = (string)mi.Invoke(null, new object[] { text })!;
        Assert.Equal("Hello", result);
    }

    [Fact]
    public async Task LoadAsync_AppendsUserInfo()
    {
        var vm = new AboutViewModel(new FakeService());
        await vm.LoadAsync();
        Assert.Contains("ACME", vm.AboutText);
    }
}
