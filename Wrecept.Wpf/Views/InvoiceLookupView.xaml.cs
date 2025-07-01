using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.ViewModels;

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

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (DataContext is InvoiceLookupViewModel vm && e.Key == Key.Up && InvoiceList.SelectedIndex == 0)
        {
            if (vm.InlinePrompt is null)
            {
                var number = DateTime.Now.ToString("yyyyMMddHHmmss");
                vm.InlinePrompt = new InvoiceCreatePromptViewModel(vm, number);
            }
            e.Handled = true;
            return;
        }
        NavigationHelper.Handle(e);
    }
}
