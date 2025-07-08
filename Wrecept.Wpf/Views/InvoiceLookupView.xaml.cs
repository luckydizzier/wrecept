using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Core.Services;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Services;
using Wrecept.Core.Enums;

namespace Wrecept.Wpf.Views;

public partial class InvoiceLookupView : UserControl
{
    private readonly FocusManager _focus;

    public InvoiceLookupView() : this(
        App.Provider.GetRequiredService<InvoiceLookupViewModel>(),
        App.Provider.GetRequiredService<FocusManager>())
    {
    }

    public InvoiceLookupView(InvoiceLookupViewModel viewModel, FocusManager focus)
    {
        InitializeComponent();
        DataContext = viewModel;
        _focus = focus;
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        try
        {
            if (DataContext is InvoiceLookupViewModel vm)
                await vm.LoadAsync();
            App.Provider.GetRequiredService<AppStateService>().InteractionState = AppInteractionState.BrowsingInvoices;
            _focus.RequestFocus(InvoiceList);
        }
        catch (Exception ex)
        {
            var log = App.Provider.GetRequiredService<ILogService>();
            await log.LogError("InvoiceLookupView.OnLoaded", ex);
        }
    }

    // billentyűkezelés a KeyboardManageren keresztül történik

}
