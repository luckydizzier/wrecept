using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf;

namespace Wrecept.Wpf.Views;

public partial class InvoiceLookupView : UserControl
{
    public InvoiceLookupView() : this(App.Provider.GetRequiredService<InvoiceLookupViewModel>())
    {
    }

    public InvoiceLookupView(InvoiceLookupViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is InvoiceLookupViewModel vm)
            await vm.LoadAsync();
    }

    private async void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (DataContext is InvoiceLookupViewModel vm)
        {
            if (vm.InlinePrompt is InvoiceCreatePromptViewModel prompt)
            {
                if (e.Key == Key.Enter)
                {
                    await prompt.ConfirmCommand.ExecuteAsync(null);
                    e.Handled = true;
                    return;
                }
                if (e.Key == Key.Escape)
                {
                    prompt.CancelCommand.Execute(null);
                    e.Handled = true;
                    return;
                }
            }

            if (e.Key == Key.Up && InvoiceList.SelectedIndex == 0)
            {
                if (vm.InlinePrompt is null)
                {
                    var number = DateTime.Now.ToString("yyyyMMddHHmmss");
                    vm.InlinePrompt = new InvoiceCreatePromptViewModel(vm, number);
                }
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Enter)
            {
                if (this.FindAncestor<InvoiceEditorView>()?.DataContext is InvoiceEditorViewModel parent)
                    await parent.OpenSelectedInvoiceCommand.ExecuteAsync(null);
                e.Handled = true;
                return;
            }
        }
        NavigationHelper.Handle(e);
    }
}
