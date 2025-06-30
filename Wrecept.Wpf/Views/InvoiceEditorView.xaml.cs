using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.ViewModels;

namespace Wrecept.Wpf.Views;

public partial class InvoiceEditorView : UserControl
{
    public InvoiceEditorView() : this(App.Provider.GetRequiredService<InvoiceEditorViewModel>())
    {
    }

    public InvoiceEditorView(InvoiceEditorViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
        => NavigationHelper.Handle(e);
}
