using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Core.Services;

namespace Wrecept.Wpf.Views.InlinePrompts;

public partial class SaveLinePromptView : UserControl
{
    public SaveLinePromptView()
    {
        InitializeComponent();
    }

    private async void OnKeyDown(object sender, KeyEventArgs e)
    {
        try
        {
            if (DataContext is not Wrecept.Wpf.ViewModels.SaveLinePromptViewModel vm)
                return;
            if (e.Key == Key.Enter)
            {
                await vm.ConfirmCommand.ExecuteAsync(null);
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                vm.CancelCommand.Execute(null);
                e.Handled = true;
            }
        }
        catch (Exception ex)
        {
            var log = App.Provider.GetRequiredService<ILogService>();
            await log.LogError("SaveLinePromptView.OnKeyDown", ex);
        }
    }
}

