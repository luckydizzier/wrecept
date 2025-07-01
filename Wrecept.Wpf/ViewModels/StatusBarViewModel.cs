using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Windows.Input;
using System.Windows.Threading;
using Wrecept.Wpf.Resources;

namespace Wrecept.Wpf.ViewModels;

public partial class StatusBarViewModel : ObservableObject
{
    private readonly DispatcherTimer _timer;

    [ObservableProperty]
    private string dateTime = string.Empty;

    [ObservableProperty]
    private string activeMenu = string.Empty;

    [ObservableProperty]
    private string focusedElement = string.Empty;

    [ObservableProperty]
    private string message = Resources.Strings.StatusBar_DefaultMessage;

    [ObservableProperty]
    private bool isNumLock;

    [ObservableProperty]
    private bool isCapsLock;

    [ObservableProperty]
    private bool isScrollLock;

    public StatusBarViewModel()
    {
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _timer.Tick += (_, _) => Update();
        _timer.Start();
        Update();
    }

    private void Update()
    {
        DateTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        IsNumLock = Keyboard.IsKeyToggled(Key.NumLock);
        IsCapsLock = Keyboard.IsKeyToggled(Key.CapsLock);
        IsScrollLock = Keyboard.IsKeyToggled(Key.Scroll);
    }
}
