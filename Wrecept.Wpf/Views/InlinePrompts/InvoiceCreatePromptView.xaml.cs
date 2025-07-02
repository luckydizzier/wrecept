using System.Windows.Controls;
using System.Windows.Input;

namespace Wrecept.Wpf.Views.InlinePrompts;

public partial class InvoiceCreatePromptView : UserControl
{
    public InvoiceCreatePromptView()
    {
        InitializeComponent();
    }

    private async void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (DataContext is not Wrecept.Wpf.ViewModels.InvoiceCreatePromptViewModel vm)
            return;
        if (e.Key == Key.Enter)
        {
            await vm.ConfirmCommand.ExecuteAsync(null);
            e.Handled = true;
        }
        else if (e.Key == Key.Escape)
        {
            vm.CancelCommand.Execute(null);
            e.Handled = true;
        }
    }
}
