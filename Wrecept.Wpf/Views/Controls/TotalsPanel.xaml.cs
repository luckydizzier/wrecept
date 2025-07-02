using System.Windows;
using System.Windows.Controls;

namespace Wrecept.Wpf.Views.Controls;

public partial class TotalsPanel : UserControl
{
    public static readonly DependencyProperty IsCompactModeProperty =
        DependencyProperty.Register(
            nameof(IsCompactMode),
            typeof(bool),
            typeof(TotalsPanel),
            new PropertyMetadata(false));

    public bool IsCompactMode
    {
        get => (bool)GetValue(IsCompactModeProperty);
        set => SetValue(IsCompactModeProperty, value);
    }

    public TotalsPanel()
    {
        InitializeComponent();
    }
}
