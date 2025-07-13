using CommunityToolkit.Mvvm.Input;

namespace InvoiceApp.MAUI.ViewModels;

public interface IEditableMasterDataViewModel : IMasterDataViewModel
{
    IRelayCommand EditSelectedCommand { get; }
    IRelayCommand DeleteSelectedCommand { get; }
    IRelayCommand CloseDetailsCommand { get; }
}
