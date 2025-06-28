namespace Wrecept.Desktop.Models;

public class SubmenuItem
{
    public int Index { get; }
    public string Label { get; }
    public Action Action { get; }

    public SubmenuItem(int index, string label, Action action)
    {
        Index = index;
        Label = label;
        Action = action;
    }
}
