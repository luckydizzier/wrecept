using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Wrecept.Wpf.ViewModels;

public partial class VatKeyEditorViewModel : ObservableObject
{
    [ObservableProperty] private string name = string.Empty;
    [ObservableProperty] private decimal percentage;

    public IRelayCommand OkCommand { get; }
    public IRelayCommand CancelCommand { get; }

    public VatKeyEditorViewModel()
    {
        OkCommand = new RelayCommand(() => OnOk?.Invoke(this));
        CancelCommand = new RelayCommand(() => OnCancel?.Invoke());
    }

    public Action<VatKeyEditorViewModel>? OnOk;
    public Action? OnCancel;
}
