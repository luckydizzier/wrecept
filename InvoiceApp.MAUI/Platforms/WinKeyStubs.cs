#if WINDOWS
namespace Microsoft.Maui.Controls
{
    public class KeyEventArgs : System.EventArgs
    {
        public Key Key { get; }
        public bool IsDown { get; }
        public KeyEventArgs(Key key, bool isDown)
        {
            Key = key;
            IsDown = isDown;
        }
    }

    public enum Key
    {
        None,
        A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
        Up,
        Down,
        Insert,
        Delete,
        Enter,
        Return,
        Escape
    }
}
#endif
