using System.Threading.Tasks;
using Xunit;
using Wrecept.Wpf.ViewModels;
using Wrecept.Core.Entities;
using Wrecept.Core.Services;

namespace Wrecept.Tests.ViewModels;

public class UserInfoViewModelTests
{
    private class FakeService : IUserInfoService
    {
        public UserInfo? Saved;
        public Task<UserInfo> LoadAsync() => Task.FromResult(new UserInfo
        {
            CompanyName = "C",
            Address = "A",
            Phone = "P",
            Email = "E",
            TaxNumber = "T",
            BankAccount = "B"
        });
        public Task SaveAsync(UserInfo info)
        {
            Saved = info;
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task LoadAsync_FillsProperties()
    {
        var service = new FakeService();
        var vm = new UserInfoViewModel(service);
        await vm.LoadAsync();
        Assert.Equal("C", vm.CompanyName);
        Assert.Equal("A", vm.Address);
        Assert.Equal("P", vm.Phone);
        Assert.Equal("E", vm.Email);
        Assert.Equal("T", vm.TaxNumber);
        Assert.Equal("B", vm.BankAccount);
    }

    [Fact]
    public async Task SaveAsync_PassesDataToService()
    {
        var service = new FakeService();
        var vm = new UserInfoViewModel(service)
        {
            CompanyName = "X",
            Address = "Y",
            Phone = "Z",
            Email = "E",
            TaxNumber = "N",
            BankAccount = "A"
        };
        await vm.SaveAsyncCommand.ExecuteAsync(null);
        Assert.NotNull(service.Saved);
        Assert.Equal("X", service.Saved!.CompanyName);
    }
}
