using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.ViewModels;

namespace Wrecept.Wpf.Views;

public partial class InvoiceEditorView : UserControl
{
    public InvoiceEditorView(InvoiceEditorViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public static InvoiceEditorView Create()
        => App.Provider.GetRequiredService<InvoiceEditorView>();

    private void OnKeyDown(object sender, KeyEventArgs e)
        => NavigationHelper.Handle(e);
}
