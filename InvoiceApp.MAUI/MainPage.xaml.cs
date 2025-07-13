using Microsoft.Maui.Controls;
using InvoiceApp.MAUI.Services;

namespace InvoiceApp.MAUI;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
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
}
