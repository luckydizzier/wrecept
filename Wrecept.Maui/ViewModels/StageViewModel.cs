namespace Wrecept.Maui;

public class StageViewModel
{
    public StageViewModel(object? a, object? b, object? c) { }

    public int SelectedIndex { get; set; }
    public int SelectedSubmenuIndex { get; set; }
    public bool IsSubMenuOpen { get; set; }
    public object? ActiveViewModel { get; set; }
    public object? Editor { get; } = new object();
}
