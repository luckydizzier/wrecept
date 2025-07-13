using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InvoiceApp.Core.Models;
using InvoiceApp.Core.Services;

namespace InvoiceApp.MAUI.ViewModels;

public partial class PaymentMethodCreatorViewModel : ObservableObject
{
    private readonly InvoiceEditorViewModel _parent;
    private readonly IPaymentMethodService _methods;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private int dueInDays;

    public PaymentMethodCreatorViewModel(InvoiceEditorViewModel parent, IPaymentMethodService methods)
    {
        _parent = parent;
        _methods = methods;
    }

    [RelayCommand]
    private async Task ConfirmAsync()
    {
        var method = new PaymentMethod { Name = Name, DueInDays = DueInDays };
        var id = await _methods.AddAsync(method);
        method.Id = id;
        _parent.PaymentMethods.Add(method);
        _parent.PaymentMethodId = method.Id;
        _parent.InlineCreator = null;
    }

    [RelayCommand]
    private void Cancel() => _parent.InlineCreator = null;
}
