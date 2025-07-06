using CommunityToolkit.Mvvm.ComponentModel;
using Wrecept.Core.Enums;
using System;

namespace Wrecept.Wpf.Services;

using System.Text.Json;
using Wrecept.Wpf.ViewModels;
using System.Threading.Tasks;
using System.IO;

public partial class AppStateService : ObservableObject
{
    private readonly string _path;

    public AppStateService(string path)
    {
        _path = path;
    }

    [ObservableProperty]
    private AppState current = AppState.None;

    [ObservableProperty]
    private AppInteractionState interactionState = AppInteractionState.None;

    public event Action<AppInteractionState>? InteractionStateChanged;

    partial void OnInteractionStateChanged(AppInteractionState value)
        => InteractionStateChanged?.Invoke(value);

    [ObservableProperty]
    private StageMenuAction lastView = StageMenuAction.InboundDeliveryNotes;

    [ObservableProperty]
    private int? currentInvoiceId;

    private record PersistedState(StageMenuAction LastView, int? InvoiceId);

    public async Task LoadAsync()
    {
        if (!File.Exists(_path))
            return;

        try
        {
            var json = await File.ReadAllTextAsync(_path);
            var data = JsonSerializer.Deserialize<PersistedState>(json);
            if (data != null)
            {
                LastView = data.LastView;
                CurrentInvoiceId = data.InvoiceId;
            }
        }
        catch (Exception)
        {
            // sérült fájl esetén indulunk alap állapotból
        }
    }

    public async Task SaveAsync()
    {
        var data = new PersistedState(LastView, CurrentInvoiceId);
        Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
        var json = JsonSerializer.Serialize(data);
        await File.WriteAllTextAsync(_path, json);
    }
}
