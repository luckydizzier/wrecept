using CommunityToolkit.Mvvm.ComponentModel;

namespace Wrecept.Wpf.ViewModels;

public partial class PlaceholderViewModel : ObservableObject
{
    [ObservableProperty]
    private string message = "Funkció fejlesztés alatt";
}
