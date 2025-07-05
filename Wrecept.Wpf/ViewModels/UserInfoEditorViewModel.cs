using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Core.Services;
using Wrecept.Wpf.Services;


namespace Wrecept.Wpf.ViewModels;

public partial class UserInfoEditorViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(OkCommand))]
    private string companyName = string.Empty;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(OkCommand))]
    private string address = string.Empty;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(OkCommand))]
    private string phone = string.Empty;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(OkCommand))]
    private string email = string.Empty;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(OkCommand))]
    private string taxNumber = string.Empty;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(OkCommand))]
    private string bankAccount = string.Empty;

    [ObservableProperty] private bool companyNameError;
    [ObservableProperty] private bool addressError;
    [ObservableProperty] private bool phoneError;
    [ObservableProperty] private bool emailError;
    [ObservableProperty] private bool taxNumberError;
    [ObservableProperty] private bool bankAccountError;

    private readonly INotificationService _notifications;

    public IRelayCommand OkCommand { get; }
    public IRelayCommand CancelCommand { get; }

    public UserInfoEditorViewModel(INotificationService? notifications = null)
    {
        _notifications = notifications ?? (App.Services != null
            ? App.Provider.GetRequiredService<INotificationService>()
            : new MessageBoxNotificationService());
        OkCommand = new RelayCommand(ExecuteOk, () => IsValid);
        CancelCommand = new RelayCommand(() => OnCancel?.Invoke());
    }

    private void ExecuteOk()
    {
        Validate();
        if (!IsValid) return;

        var summary = $"Cégnév: {CompanyName}\nCím: {Address}\nTelefonszám: {Phone}\nE-mail: {Email}\nAdószám: {TaxNumber}\nBankszámla: {BankAccount}";
        if (_notifications.Confirm($"Helyesek az adatok?\n\n{summary}"))
            OnOk?.Invoke(this);
    }

    public bool IsValid =>
        !string.IsNullOrWhiteSpace(CompanyName) &&
        !string.IsNullOrWhiteSpace(Address) &&
        !string.IsNullOrWhiteSpace(Phone) &&
        !string.IsNullOrWhiteSpace(Email) &&
        !string.IsNullOrWhiteSpace(TaxNumber) &&
        !string.IsNullOrWhiteSpace(BankAccount);

    private void Validate()
    {
        CompanyNameError = string.IsNullOrWhiteSpace(CompanyName);
        AddressError = string.IsNullOrWhiteSpace(Address);
        PhoneError = string.IsNullOrWhiteSpace(Phone);
        EmailError = string.IsNullOrWhiteSpace(Email);
        TaxNumberError = string.IsNullOrWhiteSpace(TaxNumber);
        BankAccountError = string.IsNullOrWhiteSpace(BankAccount);
    }

    public Action<UserInfoEditorViewModel>? OnOk;
    public Action? OnCancel;
}
