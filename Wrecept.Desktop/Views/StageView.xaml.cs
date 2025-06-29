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
        ViewModel = new StageViewModel(
            ServiceLocator.InvoiceService,
            ServiceLocator.ProductService,
            ServiceLocator.SupplierRepository);
        DataContext = ViewModel;
    }

}
