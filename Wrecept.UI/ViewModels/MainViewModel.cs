using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Core.Services;
using Wrecept.UI.Views;

namespace Wrecept.UI.ViewModels;

public enum MainSection
{
    Dashboard,
    Accounts,
    Stocks,
    Lists,
    Maintenance,
    Contacts
}

public class MainViewModel : INotifyPropertyChanged
{
    public ICommand DashboardCommand { get; }
    public ICommand AccountsCommand { get; }
    public ICommand StocksCommand { get; }
    public ICommand ListsCommand { get; }
    public ICommand MaintenanceCommand { get; }
    public ICommand ContactsCommand { get; }
    public ICommand ShowShortcutHelpCommand { get; }

    public ICommand EnterCommand { get; }
    public ICommand EscapeCommand { get; }
    public ICommand LeftCommand { get; }
    public ICommand RightCommand { get; }
    public ICommand UpCommand { get; }
    public ICommand DownCommand { get; }
    public ICommand ToggleThemeCommand { get; }

    private MainSection _selectedSection;
    public MainSection SelectedSection
    {
        get => _selectedSection;
        set { _selectedSection = value; OnPropertyChanged(); }
    }

    private UserControl? _currentView;
    public UserControl? CurrentView
    {
        get => _currentView;
        set { _currentView = value; OnPropertyChanged(); }
    }

    private readonly ISettingsService _settingsService;
    private string _currentTheme = "Light";

    public MainViewModel(ISettingsService settingsService)
    {
        _settingsService = settingsService;
        _ = LoadThemeAsync();

        DashboardCommand = new RelayCommand(_ => SetSection(MainSection.Dashboard));
        AccountsCommand = new RelayCommand(_ => SetSection(MainSection.Accounts));
        StocksCommand = new RelayCommand(_ => SetSection(MainSection.Stocks));
        ListsCommand = new RelayCommand(_ => SetSection(MainSection.Lists));
        MaintenanceCommand = new RelayCommand(_ => SetSection(MainSection.Maintenance));
        ContactsCommand = new RelayCommand(_ => SetSection(MainSection.Contacts));
        ShowShortcutHelpCommand = new RelayCommand(_ =>
        {
            var view = new ShortcutHelpView();
            view.Show();
        });

        EnterCommand = new RelayCommand(_ =>
        {
            if (CurrentView?.DataContext is IKeyboardNavigable nav)
                nav.OnEnter();
        });
        EscapeCommand = new RelayCommand(_ =>
        {
            if (CurrentView?.DataContext is IKeyboardNavigable nav)
                nav.OnEscape();
        });
        LeftCommand = new RelayCommand(_ => Navigate(-1));
        RightCommand = new RelayCommand(_ => Navigate(1));
        UpCommand = new RelayCommand(_ => Navigate(-1));
        DownCommand = new RelayCommand(_ => Navigate(1));
        ToggleThemeCommand = new AsyncRelayCommand(async _ =>
        {
            _currentTheme = _currentTheme == "Light" ? "Dark" : "Light";
            await _settingsService.UpdateThemeAsync(_currentTheme);
        });

        SetSection(MainSection.Dashboard);
    }

    private async Task LoadThemeAsync()
    {
        var settings = await _settingsService.LoadAsync();
        _currentTheme = string.IsNullOrEmpty(settings.Theme) ? "Light" : settings.Theme;
    }

    private UserControl CreateView<TView, TViewModel>()
        where TView : UserControl
        where TViewModel : class
    {
        var view = App.ServiceProvider.GetRequiredService<TView>();
        view.DataContext = App.ServiceProvider.GetRequiredService<TViewModel>();
        return view;
    }

    private void SetSection(MainSection section)
    {
        SelectedSection = section;
        CurrentView = section switch
        {
            MainSection.Dashboard => CreateView<DashboardView, DashboardViewModel>(),
            MainSection.Accounts => CreateView<InvoiceView, InvoiceViewModel>(),
            MainSection.Stocks => CreateView<StocksView, StocksViewModel>(),
            MainSection.Lists => CreateView<ListsView, ListsViewModel>(),
            MainSection.Maintenance => CreateView<MaintenanceView, MaintenanceViewModel>(),
            MainSection.Contacts => CreateView<ContactsView, ContactsViewModel>(),
            _ => new UserControl()
        };
    }

    private void Navigate(int direction)
    {
        var order = new[]
        {
            MainSection.Dashboard,
            MainSection.Accounts,
            MainSection.Stocks,
            MainSection.Lists,
            MainSection.Maintenance,
            MainSection.Contacts
        };
        var index = Array.IndexOf(order, SelectedSection);
        index = (index + direction + order.Length) % order.Length;
        SetSection(order[index]);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
