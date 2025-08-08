using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Wrecept.UI.ViewModels;

public class ThemeEditorViewModel : INotifyPropertyChanged
{
    public ICommand EnterCommand { get; }
    public ICommand EscapeCommand { get; }
    public ICommand LeftCommand { get; }
    public ICommand RightCommand { get; }
    public ICommand UpCommand { get; }
    public ICommand DownCommand { get; }
    public ICommand SaveCommand { get; }

    public ThemeEditorViewModel()
    {
        EnterCommand = new RelayCommand(_ => { });
        EscapeCommand = new RelayCommand(_ => { });
        LeftCommand = new RelayCommand(_ => { });
        RightCommand = new RelayCommand(_ => { });
        UpCommand = new RelayCommand(_ => { });
        DownCommand = new RelayCommand(_ => { });
        SaveCommand = new RelayCommand(_ => { });
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
