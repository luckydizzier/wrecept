using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.IO;

namespace InvoiceApp.MAUI.ViewModels;

public partial class SetupViewModel : ObservableObject
{
    [ObservableProperty]
    private string databasePath;

    [ObservableProperty]
    private string configPath;

    public IRelayCommand OkCommand { get; }
    public IRelayCommand CancelCommand { get; }

    public event Action<bool>? DialogResult;
    public IRelayCommand BrowseDbCommand { get; }
    public IRelayCommand BrowseConfigCommand { get; }

    public SetupViewModel(string dbPath, string cfgPath)
    {
        databasePath = dbPath;
        configPath = cfgPath;
        OkCommand = new RelayCommand(() => DialogResult?.Invoke(true));
        CancelCommand = new RelayCommand(() => DialogResult?.Invoke(false));
        BrowseDbCommand = new RelayCommand(OnBrowseDb);
        BrowseConfigCommand = new RelayCommand(OnBrowseConfig);
    }

    private async void OnBrowseDb()
    {
        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "SQLite DB"
        });
        if (result != null)
            DatabasePath = result.FullPath;
    }

    private async void OnBrowseConfig()
    {
        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Config JSON"
        });
        if (result != null)
            ConfigPath = result.FullPath;
    }
}
