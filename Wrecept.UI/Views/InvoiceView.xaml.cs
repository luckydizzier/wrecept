using System.Windows.Controls;
using Wrecept.UI.ViewModels;

namespace Wrecept.UI.Views;

public partial class InvoiceView : UserControl
{
    public InvoiceView(InvoiceViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
        Loaded += async (_, __) => await vm.InitializeAsync();
    }
}
