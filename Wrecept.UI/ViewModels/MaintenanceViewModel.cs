using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Core.Services;
using Wrecept.UI.Views;

namespace Wrecept.UI.ViewModels;

public class MaintenanceViewModel
{
    private readonly IExportService _exportService;

    public ICommand ExportCommand { get; }
    public ICommand ImportCommand { get; }
    public ICommand OpenThemeEditorCommand { get; }

    public MaintenanceViewModel(IExportService exportService)
    {
        _exportService = exportService;
        ExportCommand = new AsyncRelayCommand(_ => ExportAsync());
        ImportCommand = new AsyncRelayCommand(_ => ImportAsync());
        OpenThemeEditorCommand = new RelayCommand(_ => OpenThemeEditor());
    }

    private async Task ExportAsync()
    {
        var confirmMsg = Application.Current.TryFindResource("ConfirmExport") as string
                         ?? "Biztosan exportálja az adatokat?";
        var caption = Application.Current.TryFindResource("Confirmation") as string
                      ?? "Megerősítés";
        if (MessageBox.Show(confirmMsg, caption, MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            return;

        var path = Path.Combine(AppContext.BaseDirectory, "Data", "export.json");
        try
        {
            await _exportService.ExportAsync(path);
            MessageBox.Show("Exportálás kész.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Export hiba: {ex.Message}");
        }
    }

    private async Task ImportAsync()
    {
        var confirmMsg = Application.Current.TryFindResource("ConfirmImport") as string
                         ?? "Biztosan importálja az adatokat?";
        var caption = Application.Current.TryFindResource("Confirmation") as string
                      ?? "Megerősítés";
        if (MessageBox.Show(confirmMsg, caption, MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            return;

        var path = Path.Combine(AppContext.BaseDirectory, "Data", "export.json");
        try
        {
            await _exportService.ImportAsync(path);
            MessageBox.Show("Importálás kész.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Import hiba: {ex.Message}");
        }
    }

    private void OpenThemeEditor()
    {
        var window = App.ServiceProvider.GetRequiredService<ThemeEditorWindow>();
        window.ShowDialog();
    }
}
