using Microsoft.Maui.Controls;
using InvoiceApp.MAUI.Services;
using InvoiceApp.Core.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceApp.MAUI;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        RegisterKeyHandlers();
    }

    // TODO: global key handling will be wired when MAUI exposes cross-platform
    // window key events. Currently no-op to keep build clean.

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
