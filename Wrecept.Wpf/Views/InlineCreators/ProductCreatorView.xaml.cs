using System.Windows.Controls;
using System.Windows.Input;
using Wrecept.Wpf.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Wrecept.Wpf.Views.InlineCreators;

public partial class ProductCreatorView : UserControl
{
    private readonly KeyboardManager _keyboard = App.Provider.GetRequiredService<KeyboardManager>();
    public ProductCreatorView()
    {
        InitializeComponent();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (DataContext is ViewModels.ProductCreatorViewModel vm && e.Key == Key.Escape)
        {
            vm.CloseEditorCommand.Execute(null);
            e.Handled = true;
            return;
        }
        _keyboard.Handle(e);
    }
}
