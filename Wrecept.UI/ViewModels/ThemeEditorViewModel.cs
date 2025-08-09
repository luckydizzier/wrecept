using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Wrecept.Core.Services;
using Wrecept.UI.Services;

namespace Wrecept.UI.ViewModels;

public class ThemeEditorViewModel : INotifyPropertyChanged
{
    private readonly ISettingsService _settingsService;
    private readonly IMessageService _messageService;

    public ObservableCollection<string> AvailableThemes { get; } = new() { "Light", "Dark" };

    private string _selectedTheme = "Light";
    public string SelectedTheme
    {
        get => _selectedTheme;
        set { _selectedTheme = value; OnPropertyChanged(); }
    }

    public ICommand EnterCommand { get; }
    public ICommand EscapeCommand { get; }
    public ICommand LeftCommand { get; }
    public ICommand RightCommand { get; }
    public ICommand UpCommand { get; }
    public ICommand DownCommand { get; }
    public ICommand SaveCommand { get; }

    public ThemeEditorViewModel(ISettingsService settingsService, IMessageService messageService)
    {
        _settingsService = settingsService;
        _messageService = messageService;

        EnterCommand = new AsyncRelayCommand(_ => SaveAsync());
        EscapeCommand = new RelayCommand(_ => { });
        LeftCommand = new RelayCommand(_ => { });
        RightCommand = new RelayCommand(_ => { });
        UpCommand = new RelayCommand(_ => { });
        DownCommand = new RelayCommand(_ => { });
        SaveCommand = new AsyncRelayCommand(_ => SaveAsync());
    }

    public async Task InitializeAsync()
    {
        var settings = await _settingsService.LoadAsync();
        SelectedTheme = settings.Theme;
    }

    private async Task SaveAsync()
    {
        await _settingsService.UpdateThemeAsync(SelectedTheme);
        _messageService.Show("Téma elmentve.");
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
