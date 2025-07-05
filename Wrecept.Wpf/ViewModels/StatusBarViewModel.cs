using CommunityToolkit.Mvvm.ComponentModel;
using System;
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
    private string message = Resources.Strings.StatusBar_DefaultMessage;


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
    }
}
