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
    private readonly KeyboardManager? _keyboard;
    private readonly FocusManager? _focus;

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
        _keyboard = App.Provider?.GetRequiredService<KeyboardManager>();
        _focus = App.Provider?.GetRequiredService<FocusManager>();
        Keyboard.AddGotKeyboardFocusHandler(this, OnGotKeyboardFocus);
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

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            _focus?.RequestFocus(LookupView.InvoiceList);
            e.Handled = true;
            return;
        }
        _keyboard?.Handle(e);
    }

    private async void OnEntryKeyDown(object sender, KeyEventArgs e)
    {
        try
        {
            if (DataContext is not InvoiceEditorViewModel vm)
                return;

            var fe = e.OriginalSource as FrameworkElement;
            if (fe is not null)
                vm.LastFocusedField = fe.Name;

            if (e.Key == Key.Enter && fe?.Tag?.ToString() == "LastEntry")
            {
                if (vm.EditableItem.IsEditingExisting)
                    vm.SaveEditedItemCommand.Execute(null);
                else
                    await vm.AddLineItemCommand.ExecuteAsync(null);
                e.Handled = true;
                return;
            }

            if (e.Key == Key.Escape && string.IsNullOrWhiteSpace(vm.EditableItem.Product) && !vm.IsInLineFinalizationPrompt)
            {
                vm.SavePrompt = new SaveLinePromptViewModel(vm,
                    "Befejezted a tételsorok rögzítését? (Enter=Igen, Esc=Nem)",
                    finalize: true);
                vm.IsInLineFinalizationPrompt = true;
                e.Handled = true;
                return;
            }

            _keyboard?.Handle(e);
        }
        catch (Exception ex)
        {
            var log = App.Provider.GetRequiredService<ILogService>();
            await log.LogError("InvoiceEditorView.OnEntryKeyDown", ex);
        }
    }

    private void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        => _focus?.Update("InvoiceEditorView", e.NewFocus);

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
