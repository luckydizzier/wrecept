using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Core.Services;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Services;

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
            }
        }
        catch (Exception ex)
        {
            var log = App.Provider.GetRequiredService<ILogService>();
            await log.LogError("InvoiceLookupView.OnLoaded", ex);
        }
    }

}
