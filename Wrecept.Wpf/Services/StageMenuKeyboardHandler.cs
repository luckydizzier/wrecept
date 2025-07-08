using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Wrecept.Core.Enums;
using Wrecept.Wpf.ViewModels;

namespace Wrecept.Wpf.Services;

public class StageMenuKeyboardHandler : IKeyboardHandler
{
    private readonly StageViewModel _stage;
    private readonly StageMenuAction[] _actions = Enum.GetValues<StageMenuAction>();
    private int _index;

    public StageMenuKeyboardHandler(StageViewModel stage)
    {
        _stage = stage;
    }

    public bool HandleKey(KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Up:
                _index = (_index - 1 + _actions.Length) % _actions.Length;
                _stage.StatusBar.ActiveMenu = _actions[_index].ToString();
                return true;
            case Key.Down:
                _index = (_index + 1) % _actions.Length;
                _stage.StatusBar.ActiveMenu = _actions[_index].ToString();
                return true;
            case Key.Insert:
            case Key.Enter:
            case Key.Return:
                var action = _actions[_index];
                _stage.HandleMenuCommand.Execute(action);
                return true;
        }
        return false;
    }
}
