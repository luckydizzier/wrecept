using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Core.Services;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Services;
using FocusManager = Wrecept.Wpf.Services.FocusManager;

namespace Wrecept.Wpf.Views;

public partial class InvoiceLookupView : UserControl
{
    private readonly FocusManager _focus;
    public InvoiceLookupView() : this(App.Provider.GetRequiredService<InvoiceLookupViewModel>())
    {
    }

    public InvoiceLookupView(InvoiceLookupViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        _focus = App.Provider.GetRequiredService<FocusManager>();
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        try
        {
            if (DataContext is InvoiceLookupViewModel vm)
            {
                await vm.LoadAsync();
                if (vm.Invoices.Count > 0)
                    _focus.RequestFocus("InvoiceList", typeof(InvoiceLookupView));
            }
        }
        catch (Exception ex)
        {
            var log = App.Provider.GetRequiredService<ILogService>();
            await log.LogError("InvoiceLookupView.OnLoaded", ex);
        }
    }

}
