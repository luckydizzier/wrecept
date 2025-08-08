using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using Wrecept.UI.Views;

namespace Wrecept.UI.ViewModels;

public enum MainSection
{
    Accounts,
    Stocks,
    Lists,
    Maintenance,
    Contacts
}

public class MainViewModel : INotifyPropertyChanged
{
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

    public MainViewModel()
    {
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

        EnterCommand = new RelayCommand(_ => { });
        EscapeCommand = new RelayCommand(_ => { });
        LeftCommand = new RelayCommand(_ => { });
        RightCommand = new RelayCommand(_ => { });
        UpCommand = new RelayCommand(_ => { });
        DownCommand = new RelayCommand(_ => { });

        SetSection(MainSection.Accounts);
    }

    private void SetSection(MainSection section)
    {
        SelectedSection = section;
        CurrentView = section switch
        {
            MainSection.Accounts => new InvoiceEditorView(),
            MainSection.Stocks => new StocksView(),
            MainSection.Lists => new ListsView(),
            MainSection.Maintenance => new MaintenanceView(),
            MainSection.Contacts => new ContactsView(),
            _ => null
        };
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
