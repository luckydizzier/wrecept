using System.ComponentModel;
using System.Windows;
using Wrecept.UI.Services;

namespace Wrecept.UI;

public partial class MainWindow : Window
{
    private readonly IMessageService _messageService;

    public MainWindow(IMessageService messageService)
    {
        _messageService = messageService;
        InitializeComponent();
        Closing += OnClosing;
    }

    private void OnClosing(object? sender, CancelEventArgs e)
    {
        string msg = Application.Current.TryFindResource("ConfirmExitMessage") as string
                      ?? "Biztosan kilépsz az alkalmazásból?";
        string title = Application.Current.TryFindResource("ConfirmExitTitle") as string
                        ?? "Kilépés megerősítése";
        if (!_messageService.Confirm(msg, title))
        {
            e.Cancel = true;
        }
    }
}
