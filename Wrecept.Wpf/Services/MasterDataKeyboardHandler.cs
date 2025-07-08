using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Wrecept.Wpf.ViewModels;

namespace Wrecept.Wpf.Services;

public class MasterDataKeyboardHandler : IKeyboardHandler
{
    private readonly StageViewModel _stage;

    public MasterDataKeyboardHandler(StageViewModel stage)
    {
        _stage = stage;
    }

    public bool HandleKey(KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Insert:
            case Key.Enter or Key.Return:
                if (_stage.CurrentViewModel is IEditableMasterDataViewModel vmEdit)
                    vmEdit.EditSelectedCommand.Execute(null);
                return true;
            case Key.Delete:
                if (_stage.CurrentViewModel is IEditableMasterDataViewModel vmDel)
                    vmDel.DeleteSelectedCommand.Execute(null);
                return true;
            case Key.Escape:
                if (_stage.CurrentViewModel is IEditableMasterDataViewModel vmClose)
                    vmClose.CloseDetailsCommand.Execute(null);
                return true;
        }
        return false;
    }
}
