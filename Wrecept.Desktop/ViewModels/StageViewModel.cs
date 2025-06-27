using CommunityToolkit.Mvvm.ComponentModel;

namespace Wrecept.Desktop.ViewModels;

public partial class StageViewModel : ObservableObject
{
    public MainMenuViewModel Menu { get; } = new();
    public InvoiceEditorViewModel Editor { get; } = new();

    [ObservableProperty]
    private bool showEditor;

    public StageViewModel()
    {
        Menu.ItemActivated += idx => ShowEditor = idx == 0;
    }
}
