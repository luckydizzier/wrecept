namespace Wrecept.Wpf;

using System.Windows.Controls;

public static class KeyboardOnlyUXHelper
{
    public static void SuppressUnintendedDropDown(ComboBox combo)
    {
        ArgumentNullException.ThrowIfNull(combo);
        combo.PreviewMouseDown += (_, e) => e.Handled = true;
    }
}
