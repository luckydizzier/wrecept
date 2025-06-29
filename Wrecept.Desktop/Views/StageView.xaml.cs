using System.Windows;
using System.Windows.Controls;
using Wrecept.Desktop.ViewModels;
using Wrecept.Desktop;

namespace Wrecept.Desktop.Views;

public partial class StageView : UserControl
{
    public StageViewModel ViewModel { get; }


    public StageView()
    {
        InitializeComponent();
        ViewModel = new StageViewModel(ServiceLocator.InvoiceService);
        DataContext = ViewModel;
    }

    private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        MainMenuFirstButton.Focus();
    }

    private void MainMenuButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        if (sender is Button btn && int.TryParse(btn.Tag?.ToString(), out var index))
        {
            ViewModel.SelectedIndex = index;
            ViewModel.SelectedSubmenuIndex = 0;
            ViewModel.IsSubMenuOpen = true;
        }
    }

    private void SubMenuButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        if (sender is Button btn && int.TryParse(btn.Tag?.ToString(), out var subIndex))
        {
            if (btn.Parent is FrameworkElement fe && int.TryParse(fe.Tag?.ToString(), out var mainIndex))
            {
                ViewModel.SelectedIndex = mainIndex;
                ViewModel.SelectedSubmenuIndex = subIndex;
                var mainVm = (Wrecept.Desktop.ViewModels.MainWindowViewModel?)Application.Current.MainWindow?.DataContext;
                mainVm?.EnterCommand.Execute(null);
            }
        }
    }
}
