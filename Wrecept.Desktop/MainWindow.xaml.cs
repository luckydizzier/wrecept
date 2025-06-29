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

    // korábbi Click-kezelők helyett parancskötéseket használunk
}
