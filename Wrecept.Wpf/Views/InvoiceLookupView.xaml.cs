using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Core.Services;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Services;
using FocusService = Wrecept.Wpf.Services.FocusManager;

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
        try
        {
            if (DataContext is InvoiceLookupViewModel vm)
            {
                await vm.LoadAsync();
                var focus = App.Provider.GetRequiredService<FocusService>();
                focus.RequestFocus(InvoiceList);
            }
        }
        catch (Exception ex)
        {
            var log = App.Provider.GetRequiredService<ILogService>();
            await log.LogError("InvoiceLookupView.OnLoaded", ex);
        }
    }

    private async void InvoiceList_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (DataContext is not InvoiceLookupViewModel vm)
            return;

        if (e.Key == System.Windows.Input.Key.Insert)
        {
            await vm.PromptNewInvoiceAsync();
            e.Handled = true;
        }
        else if (e.Key == System.Windows.Input.Key.Up && InvoiceList.SelectedIndex == 0)
        {
            await vm.PromptNewInvoiceAsync();
            e.Handled = true;
        }
    }

}
