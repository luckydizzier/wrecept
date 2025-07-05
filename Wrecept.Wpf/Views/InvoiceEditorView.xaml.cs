using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Input;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.ViewModels;
using Wrecept.Core.Services;
using Wrecept.Core.Utilities;
using Wrecept.Wpf;
using Wrecept.Wpf.Services;

namespace Wrecept.Wpf.Views;

public partial class InvoiceEditorView : UserControl
{

    public InvoiceEditorView()
    {
        if (App.Provider is null)
        {
            InitializeComponent();
            return;
        }
        Initialize(App.Provider.GetRequiredService<InvoiceEditorViewModel>());
    }

    public InvoiceEditorView(InvoiceEditorViewModel viewModel)
    {
        Initialize(viewModel);
    }

    private void Initialize(InvoiceEditorViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        KeyboardOnlyUXHelper.SuppressUnintendedDropDown(PaymentLookup.BoxControl);
        PaymentLookup.BoxControl.DropDownOpened += PaymentLookup_DropDownOpened;
        if (DataContext is InvoiceEditorViewModel vm)
            vm.PropertyChanged += Vm_PropertyChanged;

        Loaded += async (_, _) =>
        {
            var progressVm = new ProgressViewModel();
            var progressWindow = new StartupWindow { DataContext = progressVm };
            progressWindow.Show();
            var progress = new Progress<ProgressReport>(r =>
            {
                progressVm.GlobalProgress = r.SubtaskPercent;
                progressVm.SubProgress = r.SubtaskPercent;
                progressVm.StatusMessage = r.Message;
            });
            await viewModel.LoadAsync(progress);
            progressWindow.Close();
        };
    }

    private void PaymentLookup_DropDownOpened(object? sender, EventArgs e)
    {
        if (!PaymentLookup.BoxControl.IsKeyboardFocusWithin)
            PaymentLookup.BoxControl.IsDropDownOpen = false;
    }

    private void Vm_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (sender is not InvoiceEditorViewModel vm)
            return;

        if (e.PropertyName == nameof(vm.Supplier))
        {
            SupplierLookup.Text = vm.Supplier;
        }
        else if (e.PropertyName == nameof(vm.PaymentMethodId))
        {
            var match = vm.PaymentMethods.FirstOrDefault(p => p.Id == vm.PaymentMethodId);
            if (match != null)
                PaymentLookup.BoxControl.SelectedItem = match;
            PaymentLookup.BoxControl.IsDropDownOpen = false;
        }
        else if (e.PropertyName == nameof(vm.InvoiceId))
        {
            LookupView.InvoiceList.Focus();
            Keyboard.Focus(LookupView.InvoiceList);
        }
    }
}
