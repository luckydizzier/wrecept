using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace Wrecept.Wpf.ViewModels;

public partial class ProgressViewModel : ObservableObject
{
    [ObservableProperty]
    private int globalProgress;

    [ObservableProperty]
    private int subProgress;

    [ObservableProperty]
    private string statusMessage = string.Empty;

    public ICommand? CancelCommand { get; set; }
}
