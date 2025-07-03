using CommunityToolkit.Mvvm.Input;

namespace Wrecept.Wpf.ViewModels;

public interface IEditableMasterDataViewModel : IMasterDataViewModel
{
    IRelayCommand EditSelectedCommand { get; }
    IRelayCommand DeleteSelectedCommand { get; }
    IRelayCommand CloseDetailsCommand { get; }
}
