using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wrecept.Core.Entities;
using Wrecept.Core.Services;

namespace Wrecept.Wpf.ViewModels;

public partial class UserInfoViewModel : ObservableObject
{
    private readonly IUserInfoService _service;

    [ObservableProperty] private string companyName = string.Empty;
    [ObservableProperty] private string address = string.Empty;
    [ObservableProperty] private string phone = string.Empty;
    [ObservableProperty] private string email = string.Empty;
    [ObservableProperty] private string taxNumber = string.Empty;
    [ObservableProperty] private string bankAccount = string.Empty;

    public UserInfoViewModel(IUserInfoService service)
    {
        _service = service;
    }

    public async Task LoadAsync()
    {
        var info = await _service.LoadAsync();
        CompanyName = info.CompanyName;
        Address = info.Address;
        Phone = info.Phone;
        Email = info.Email;
        TaxNumber = info.TaxNumber;
        BankAccount = info.BankAccount;
    }

    [RelayCommand]
    public async Task SaveAsync()
    {
        await _service.SaveAsync(new UserInfo
        {
            CompanyName = CompanyName,
            Address = Address,
            Phone = Phone,
            Email = Email,
            TaxNumber = TaxNumber,
            BankAccount = BankAccount
        });
    }
}
