using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.ViewModels;
using Wrecept.Core.Utilities;

namespace Wrecept.Wpf.Views;

public partial class InvoiceEditorView : UserControl
{
    public InvoiceEditorView() : this(App.Provider.GetRequiredService<InvoiceEditorViewModel>())
    {
    }

    public InvoiceEditorView(InvoiceEditorViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
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
            LookupView.InvoiceList.Focus();
        };
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            LookupView.InvoiceList.Focus();
            e.Handled = true;
            return;
        }
        NavigationHelper.Handle(e);
    }

    private async void OnEntryKeyDown(object sender, KeyEventArgs e)
    {
        if (DataContext is not InvoiceEditorViewModel vm)
            return;

        if (e.OriginalSource is FrameworkElement fe)
            vm.LastFocusedField = fe.Name;

        if (e.Key == Key.Enter && fe.Name == "EntryTax")
        {
            await vm.AddLineItemCommand.ExecuteAsync(null);
            e.Handled = true;
            return;
        }

        if (e.Key == Key.Escape && vm.Items.Count > 1 && !vm.IsInLineFinalizationPrompt)
        {
            vm.SavePrompt = new SaveLinePromptViewModel(vm,
                "Befejezted a tételsorok rögzítését? (Enter=Igen, Esc=Nem)",
                finalize: true);
            vm.IsInLineFinalizationPrompt = true;
            e.Handled = true;
            return;
        }

        NavigationHelper.Handle(e);
    }
}
