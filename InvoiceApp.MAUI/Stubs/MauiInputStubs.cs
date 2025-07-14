namespace Microsoft.Maui.Input;

public enum Key
{
    None,
    Up,
    Down,
    Left,
    Right,
    Enter,
    Return,
    Escape,
    Insert,
    Delete,
    A,
    B
}

public class KeyEventArgs(Key key, bool isRepeat = false) : EventArgs
{
    public Key Key { get; } = key;
    public bool IsRepeat { get; } = isRepeat;
    public bool Handled { get; set; }
}

