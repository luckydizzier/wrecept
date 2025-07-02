using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace Wrecept.Wpf.ViewModels;

public partial class SetupViewModel : ObservableObject
{
    [ObservableProperty]
    private string databasePath;

    [ObservableProperty]
    private string configPath;

    public IRelayCommand<Window?> OkCommand { get; }
    public IRelayCommand<Window?> CancelCommand { get; }
    public IRelayCommand BrowseDbCommand { get; }
    public IRelayCommand BrowseConfigCommand { get; }

    public SetupViewModel(string dbPath, string cfgPath)
    {
        databasePath = dbPath;
        configPath = cfgPath;
        OkCommand = new RelayCommand<Window?>(w => { if (w != null) { w.DialogResult = true; w.Close(); } });
        CancelCommand = new RelayCommand<Window?>(w => { if (w != null) { w.DialogResult = false; w.Close(); } });
        BrowseDbCommand = new RelayCommand(OnBrowseDb);
        BrowseConfigCommand = new RelayCommand(OnBrowseConfig);
    }

    private void OnBrowseDb()
    {
        var dlg = new SaveFileDialog
        {
            Filter = "SQLite DB (*.db)|*.db|All files|*.*",
            FileName = Path.GetFileName(DatabasePath),
            InitialDirectory = Path.GetDirectoryName(DatabasePath)
        };
        if (dlg.ShowDialog() == true)
            DatabasePath = dlg.FileName;
    }

    private void OnBrowseConfig()
    {
        var dlg = new SaveFileDialog
        {
            Filter = "JSON (*.json)|*.json|All files|*.*",
            FileName = Path.GetFileName(ConfigPath),
            InitialDirectory = Path.GetDirectoryName(ConfigPath)
        };
        if (dlg.ShowDialog() == true)
            ConfigPath = dlg.FileName;
    }
}
