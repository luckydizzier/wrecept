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

    private async void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (DataContext is InvoiceLookupViewModel vm && e.Key == Key.Up && InvoiceList.SelectedIndex == 0)
        {
            var number = DateTime.Now.ToString("yyyyMMddHHmmss");
            var result = MessageBox.Show($"Új számla {number}?", "Számlalétrehozás", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                await vm.CreateInvoiceAsync(number);
            }
            e.Handled = true;
            return;
        }
        NavigationHelper.Handle(e);
    }
}
