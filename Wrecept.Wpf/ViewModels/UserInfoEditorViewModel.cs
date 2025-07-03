using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Wrecept.Wpf.ViewModels;

public partial class UserInfoEditorViewModel : ObservableObject
{
    [ObservableProperty] private string companyName = string.Empty;
    [ObservableProperty] private string address = string.Empty;
    [ObservableProperty] private string phone = string.Empty;
    [ObservableProperty] private string email = string.Empty;

    public IRelayCommand OkCommand { get; }
    public IRelayCommand CancelCommand { get; }

    public UserInfoEditorViewModel()
    {
        OkCommand = new RelayCommand(() => OnOk?.Invoke(this), () => IsValid);
        CancelCommand = new RelayCommand(() => OnCancel?.Invoke());
    }

    public bool IsValid => !string.IsNullOrWhiteSpace(CompanyName);

    public Action<UserInfoEditorViewModel>? OnOk;
    public Action? OnCancel;
}
