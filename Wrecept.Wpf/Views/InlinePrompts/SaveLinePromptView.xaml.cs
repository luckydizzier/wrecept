using System.Windows.Controls;
using System.Windows.Input;

namespace Wrecept.Wpf.Views.InlinePrompts;

public partial class SaveLinePromptView : UserControl
{
    public SaveLinePromptView()
    {
        InitializeComponent();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (DataContext is not Wrecept.Wpf.ViewModels.SaveLinePromptViewModel vm)
            return;
        if (e.Key == Key.Enter)
        {
            vm.ConfirmCommand.Execute(null);
            e.Handled = true;
        }
        else if (e.Key == Key.Escape)
        {
            vm.CancelCommand.Execute(null);
            e.Handled = true;
        }
    }
}

