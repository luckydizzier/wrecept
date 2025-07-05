using System.Windows.Controls;

namespace Wrecept.Wpf;

public static class KeyboardOnlyUXHelper
{
    public static void SuppressUnintendedDropDown(ComboBox combo)
    {
        combo.PreviewMouseDown += (_, e) => e.Handled = true;
        combo.DropDownOpened += (_, e) =>
        {
            if (!combo.IsKeyboardFocusWithin)
                combo.IsDropDownOpen = false;
        };
    }
}
