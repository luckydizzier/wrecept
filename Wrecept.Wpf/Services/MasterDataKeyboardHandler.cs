using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Wrecept.Wpf.ViewModels;

namespace Wrecept.Wpf.Services;

public class MasterDataKeyboardHandler : IKeyboardHandler
{
    private readonly IEditableMasterDataViewModel _vm;

    public MasterDataKeyboardHandler(IEditableMasterDataViewModel vm)
    {
        _vm = vm;
    }

    public bool HandleKey(KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Insert:
            case Key.Enter:
                _vm.EditSelectedCommand.Execute(null);
                return true;
            case Key.Delete:
                _vm.DeleteSelectedCommand.Execute(null);
                return true;
            case Key.Escape:
                _vm.CloseDetailsCommand.Execute(null);
                return true;
        }
        return false;
    }
}
