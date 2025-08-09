using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using Wrecept.UI.ViewModels;

namespace Wrecept.UI.Views;

public partial class ThemeEditorView : UserControl
{
    public ThemeEditorView()
    {
        InitializeComponent();
        var vm = App.ServiceProvider.GetRequiredService<ThemeEditorViewModel>();
        DataContext = vm;
        Loaded += async (_, __) => await vm.InitializeAsync();
    }
}
