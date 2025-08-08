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
        ExportCommand = new RelayCommand(async _ => await ExportAsync());
        ImportCommand = new RelayCommand(async _ => await ImportAsync());
        OpenThemeEditorCommand = new RelayCommand(_ => OpenThemeEditor());
    }

    private async Task ExportAsync()
    {
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
