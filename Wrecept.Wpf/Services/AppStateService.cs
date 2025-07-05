using CommunityToolkit.Mvvm.ComponentModel;
using Wrecept.Core.Enums;

namespace Wrecept.Wpf.Services;

public partial class AppStateService : ObservableObject
{
    [ObservableProperty]
    private AppState current = AppState.None;
}
