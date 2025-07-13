using Microsoft.Maui.Controls;
using InvoiceApp.MAUI.Services;
using Wrecept.Core.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceApp.MAUI;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        RegisterKeyHandlers();
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        if (Handler?.MauiContext?.Window is IWindow window)
        {
            window.KeyDown += OnKeyDown;
        }
    }

    private void OnKeyDown(object? sender, Microsoft.Maui.Input.KeyEventArgs e)
    {
        var km = MauiProgram.Services?.GetService<KeyboardManager>();
        if (km != null && km.Process(e))
            e.Handled = true;
    }

    private static void RegisterKeyHandlers()
    {
        if (MauiProgram.Services is not IServiceProvider sp)
            return;

        var km = sp.GetRequiredService<KeyboardManager>();
        km.Register(AppInteractionState.MainMenu, sp.GetRequiredService<StageMenuKeyboardHandler>());
        km.Register(AppInteractionState.EditingMasterData, sp.GetRequiredService<MasterDataKeyboardHandler>());
        km.Register(AppInteractionState.EditingInvoice, sp.GetRequiredService<InvoiceEditorKeyboardHandler>());
        km.Register(AppInteractionState.BrowsingInvoices, sp.GetRequiredService<InvoiceLookupKeyboardHandler>());
    }
}
