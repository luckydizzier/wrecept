using Wrecept.Desktop.ViewModels;

namespace Wrecept.Tests;

public class MainWindowViewModelTests
{
    [Fact]
    public void EnterCommand_OnInvoiceMenu_OpensEditor()
    {
        var stage = new StageViewModel();
        var vm = new MainWindowViewModel(stage);

        stage.SelectedIndex = 0;
        stage.SelectedSubmenuIndex = 1;
        stage.IsSubMenuOpen = true;

        vm.EnterCommand.Execute(null);

        Assert.True(stage.ShowEditor);
    }
}
