using System.Windows.Controls;
using System.Windows.Input;
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
        };
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
        => NavigationHelper.Handle(e);
}
