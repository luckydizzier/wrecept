using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Timers;
using Microsoft.Maui.ApplicationModel;
using InvoiceApp.MAUI.Resources;

namespace InvoiceApp.MAUI.ViewModels;

public partial class StatusBarViewModel : ObservableObject
{
    private readonly System.Timers.Timer _timer;

    [ObservableProperty]
    private string dateTime = string.Empty;

    [ObservableProperty]
    private string activeMenu = string.Empty;


    [ObservableProperty]
    private string message = Resources.Strings.StatusBar_DefaultMessage;


    public StatusBarViewModel()
    {
        _timer = new System.Timers.Timer(1000)
        {
            AutoReset = true
        };
        _timer.Elapsed += (_, _) => Update();
        _timer.Start();
        Update();
    }

    private void Update()
    {
        MainThread.BeginInvokeOnMainThread(() =>
            DateTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    }
}
