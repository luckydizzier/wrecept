using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wrecept.Desktop;

public partial class MainWindow : Window
{
    public ViewModels.MainWindowViewModel ViewModel { get; }

    public MainWindow()
    {
        InitializeComponent();
        var stageVm = new ViewModels.StageViewModel(
            ServiceLocator.InvoiceService,
            ServiceLocator.ProductService,
            ServiceLocator.SupplierRepository);
        Stage.ViewModel = stageVm;
        ViewModel = new ViewModels.MainWindowViewModel(stageVm);
        DataContext = ViewModel;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        MainMenuFirstItem.Focus();
    }

    private void MainMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (sender is MenuItem mi && int.TryParse(mi.Tag?.ToString(), out var index))
        {
            ViewModel.Stage.SelectedIndex = index;
            ViewModel.Stage.SelectedSubmenuIndex = 0;
            ViewModel.Stage.IsSubMenuOpen = true;
        }
    }

    private void SubMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (sender is MenuItem mi && int.TryParse(mi.Tag?.ToString(), out var subIndex))
        {
            if (mi.Parent is MenuItem parent && int.TryParse(parent.Tag?.ToString(), out var mainIndex))
            {
                ViewModel.Stage.SelectedIndex = mainIndex;
                ViewModel.Stage.SelectedSubmenuIndex = subIndex;
                ViewModel.EnterCommand.Execute(null);
            }
        }
    }
}
