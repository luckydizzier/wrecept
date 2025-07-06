using System;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Core.Services;
using Wrecept.Core.Utilities;
using Wrecept.Wpf.Services;
using Wrecept.Wpf.ViewModels;

namespace Wrecept.Wpf.Views;

public partial class InvoiceEditorLayout : UserControl
{
    public InvoiceEditorLayout()
    {
        if (App.Provider is null)
        {
            InitializeComponent();
            return;
        }
        Initialize(App.Provider.GetRequiredService<InvoiceEditorViewModel>());
    }

    public InvoiceEditorLayout(InvoiceEditorViewModel viewModel)
    {
        Initialize(viewModel);
    }

    private void Initialize(InvoiceEditorViewModel viewModel)
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
        };
    }

    private void LastEntry_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == System.Windows.Input.Key.Escape && DataContext is InvoiceEditorViewModel vm)
        {
            if (vm.IsInLineFinalizationPrompt)
                return;

            vm.IsInLineFinalizationPrompt = true;
            vm.SavePrompt = new SaveLinePromptViewModel(vm,
                "Végeztél a szerkesztéssel? (Enter=Igen, Esc=Nem)", finalize: true);
            e.Handled = true;
        }
    }

    private async void EntryDesc_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == System.Windows.Input.Key.Enter && DataContext is InvoiceEditorViewModel vm)
        {
            vm.ShowSavePromptCommand.Execute(null);
            await vm.AddLineItemCommand.ExecuteAsync(null);
            EntryProduct.Focus();
            e.Handled = true;
        }
    }
}

