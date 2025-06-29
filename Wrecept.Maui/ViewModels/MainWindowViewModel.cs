namespace Wrecept.Maui;

using System.Windows.Input;

public class MainWindowViewModel
{
    private readonly StageViewModel _stage;

    public MainWindowViewModel(StageViewModel stage)
    {
        _stage = stage;
        EnterCommand = new SimpleCommand(Enter);
    }

    public ICommand EnterCommand { get; }

    private void Enter()
    {
        if (_stage.IsSubMenuOpen && _stage.SelectedIndex == 0 && _stage.SelectedSubmenuIndex == 1)
            _stage.ActiveViewModel = _stage.Editor;
    }

    private class SimpleCommand : ICommand
    {
        private readonly Action _execute;
        public SimpleCommand(Action execute) => _execute = execute;
        public event EventHandler? CanExecuteChanged;
        public bool CanExecute(object? parameter) => true;
        public void Execute(object? parameter) => _execute();
    }
}
