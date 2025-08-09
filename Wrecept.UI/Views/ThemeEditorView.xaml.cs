using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using Wrecept.UI.ViewModels;

namespace Wrecept.UI.Views;

public partial class ThemeEditorView : UserControl
{
    public ThemeEditorView()
    {
        InitializeComponent();
        DataContext = App.ServiceProvider.GetRequiredService<ThemeEditorViewModel>();
    }
}
