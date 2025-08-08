using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using Wrecept.UI.ViewModels;

namespace Wrecept.UI.Views;

public partial class InvoiceEditorView : UserControl
{
    public InvoiceEditorView()
    {
        InitializeComponent();
    }

    private InvoiceEditorViewModel? Vm => DataContext as InvoiceEditorViewModel;

    private void TbSearch_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (Vm == null) return;

        if (e.Key == Key.Down && Vm.HasSuggestions)
        {
            lstSuggestions.SelectedIndex = 0;
            lstSuggestions.Focus();
            e.Handled = true;
        }
        else if (e.Key == Key.Up && Vm.HasSuggestions)
        {
            if (lstSuggestions.Items.Count > 0)
            {
                lstSuggestions.SelectedIndex = lstSuggestions.Items.Count - 1;
                lstSuggestions.Focus();
                e.Handled = true;
            }
        }
        else if (e.Key == Key.Enter)
        {
            if (Vm.HasSuggestions)
            {
                var suggestion = lstSuggestions.SelectedItem as string ?? lstSuggestions.Items.Cast<object>().FirstOrDefault() as string;
                if (suggestion != null)
                {
                    Vm.SelectSuggestionCommand.Execute(suggestion);
                }
                dgInvoiceItems.Focus();
            }
            else
            {
                Vm.AddItemCommand.Execute(null);
                dgInvoiceItems.Focus();
            }
            e.Handled = true;
        }
        else if (e.Key == Key.Escape)
        {
            if (Vm.HasSuggestions)
            {
                Vm.CloseSuggestionsCommand.Execute(null);
            }
            dgInvoiceItems.Focus();
            e.Handled = true;
        }
    }

    private void LstSuggestions_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (Vm == null) return;

        if (e.Key == Key.Enter)
        {
            var suggestion = lstSuggestions.SelectedItem as string;
            if (suggestion != null)
            {
                Vm.SelectSuggestionCommand.Execute(suggestion);
            }
            dgInvoiceItems.Focus();
            e.Handled = true;
        }
        else if (e.Key == Key.Up && lstSuggestions.SelectedIndex == 0)
        {
            tbSearch.Focus();
            e.Handled = true;
        }
        else if (e.Key == Key.Down && lstSuggestions.SelectedIndex == lstSuggestions.Items.Count - 1)
        {
            tbSearch.Focus();
            e.Handled = true;
        }
        else if (e.Key == Key.Escape)
        {
            Vm.CloseSuggestionsCommand.Execute(null);
            dgInvoiceItems.Focus();
            e.Handled = true;
        }
    }
}
