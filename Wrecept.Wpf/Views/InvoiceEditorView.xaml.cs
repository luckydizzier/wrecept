using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.ViewModels;
using Wrecept.Core.Services;
using Wrecept.Core.Utilities;
using Wrecept.Wpf;
using Wrecept.Wpf.Services;
using FocusManager = Wrecept.Wpf.Services.FocusManager;

namespace Wrecept.Wpf.Views;

public partial class InvoiceEditorView : UserControl
{
    private FocusManager? _focus;

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
        _focus = App.Provider?.GetRequiredService<FocusManager>();
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
            _ = Dispatcher.InvokeAsync(() =>
                _focus?.RequestFocus("InvoiceList", typeof(InvoiceEditorView)),
                DispatcherPriority.ContextIdle);
        };
    }


    private void OnInlineCreatorOpened(object sender, EventArgs e)
    {
        if (!InlineCreatorHost.IsVisible)
            return;

        Dispatcher.BeginInvoke(() =>
        {
            if (InlineCreatorHost.Content is FrameworkElement fe)
            {
                if (fe.FindName("NameBox") is IInputElement box)
                    _focus?.RequestFocus(box);
                else
                    _focus?.RequestFocus(fe);
            }
        }, DispatcherPriority.Background);
    }
}
