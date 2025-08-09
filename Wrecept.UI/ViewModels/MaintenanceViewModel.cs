using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Core.Services;
using Wrecept.UI.Services;
using Wrecept.UI.Views;

namespace Wrecept.UI.ViewModels;

public class MaintenanceViewModel
{
    private readonly IExportService _exportService;
    private readonly IMessageService _messageService;

    public ICommand ExportCommand { get; }
    public ICommand ImportCommand { get; }
    public ICommand OpenThemeEditorCommand { get; }

    public MaintenanceViewModel(IExportService exportService, IMessageService messageService)
    {
        _exportService = exportService;
        _messageService = messageService;
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
        if (!_messageService.Confirm(confirmMsg, caption))
            return;

        var path = Path.Combine(AppContext.BaseDirectory, "Data", "export.json");
        try
        {
            await _exportService.ExportAsync(path);
            _messageService.Show("Exportálás kész.");
        }
        catch (Exception ex)
        {
            _messageService.Show($"Export hiba: {ex.Message}");
        }
    }

    private async Task ImportAsync()
    {
        var confirmMsg = Application.Current.TryFindResource("ConfirmImport") as string
                         ?? "Biztosan importálja az adatokat?";
        var caption = Application.Current.TryFindResource("Confirmation") as string
                      ?? "Megerősítés";
        if (!_messageService.Confirm(confirmMsg, caption))
            return;

        var path = Path.Combine(AppContext.BaseDirectory, "Data", "export.json");
        try
        {
            await _exportService.ImportAsync(path);
            _messageService.Show("Importálás kész.");
        }
        catch (Exception ex)
        {
            _messageService.Show($"Import hiba: {ex.Message}");
        }
    }

    private void OpenThemeEditor()
    {
        var window = App.ServiceProvider.GetRequiredService<ThemeEditorWindow>();
        window.ShowDialog();
    }
}
