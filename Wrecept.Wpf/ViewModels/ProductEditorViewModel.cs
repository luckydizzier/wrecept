using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Wrecept.Wpf.ViewModels;

public partial class ProductEditorViewModel : ObservableObject
{
    [ObservableProperty] private string name = string.Empty;
    [ObservableProperty] private decimal net;
    [ObservableProperty] private decimal gross;
    [ObservableProperty] private Guid taxRateId;

    public IRelayCommand OkCommand { get; }
    public IRelayCommand CancelCommand { get; }

    public ProductEditorViewModel()
    {
        OkCommand = new RelayCommand(() => OnOk?.Invoke(this));
        CancelCommand = new RelayCommand(() => OnCancel?.Invoke());
    }

    public Action<ProductEditorViewModel>? OnOk;
    public Action? OnCancel;
}
