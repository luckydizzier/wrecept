using CommunityToolkit.Mvvm.ComponentModel;

namespace InvoiceApp.MAUI.ViewModels;

public partial class PlaceholderViewModel : ObservableObject
{
    [ObservableProperty]
    private string message = "Funkció fejlesztés alatt";
}
