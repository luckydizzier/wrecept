using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Wrecept.UI.ViewModels;

public class ShortcutHelpViewModel : INotifyPropertyChanged
{
    private readonly Window _window;

    public ICommand EnterCommand { get; }
    public ICommand EscapeCommand { get; }
    public ICommand LeftCommand { get; }
    public ICommand RightCommand { get; }
    public ICommand UpCommand { get; }
    public ICommand DownCommand { get; }

    public ShortcutHelpViewModel(Window window)
    {
        _window = window;
        EnterCommand = new RelayCommand(_ => _window.Close());
        EscapeCommand = new RelayCommand(_ => _window.Close());
        LeftCommand = new RelayCommand(_ => { });
        RightCommand = new RelayCommand(_ => { });
        UpCommand = new RelayCommand(_ => { });
        DownCommand = new RelayCommand(_ => { });
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
