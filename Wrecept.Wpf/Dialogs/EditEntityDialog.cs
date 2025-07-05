using System.Windows;
using CommunityToolkit.Mvvm.Input;

namespace Wrecept.Wpf.Dialogs;

public class EditEntityDialog<TView, TViewModel> : Window
    where TView : FrameworkElement, new()
    where TViewModel : class
{
    public TViewModel ViewModel { get; }

    public EditEntityDialog(TViewModel viewModel)
    {
        ViewModel = viewModel;
        var view = new TView { DataContext = viewModel };
        Content = view;
        WindowStyle = WindowStyle.ToolWindow;
        SizeToContent = SizeToContent.WidthAndHeight;
    }

    public void Initialize(IRelayCommand ok, IRelayCommand cancel)
    {
        DialogHelper.CenterToOwner(this, Application.Current.MainWindow!);
    }
}
