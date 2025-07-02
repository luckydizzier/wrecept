using CommunityToolkit.Mvvm.Input;
using Wrecept.Wpf.Dialogs;

namespace Wrecept.Wpf.Services;

public static class DialogService
{
    public static bool EditEntity<TView, TViewModel>(TViewModel viewModel,
        IRelayCommand okCommand, IRelayCommand cancelCommand)
        where TView : System.Windows.FrameworkElement, new()
        where TViewModel : class
    {
        var dlg = new EditEntityDialog<TView, TViewModel>(viewModel);
        dlg.Initialize(okCommand, cancelCommand);
        return NavigationService.ShowCenteredDialog(dlg);
    }
}
