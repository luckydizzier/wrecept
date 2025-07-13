using InvoiceApp.MAUI.ViewModels;
using InvoiceApp.MAUI.Views.Dialogs;
using Microsoft.Maui.Controls;

namespace InvoiceApp.MAUI.Services;

public class SetupFlow : ISetupFlow
{
    public async Task<SetupData> RunAsync(string defaultDb, string defaultCfg)
    {
        var vm = new SetupViewModel(defaultDb, defaultCfg);
        var page = new SetupPage { BindingContext = vm };
        var result = await ShowDialogAsync(page, vm);
        if (!result)
            throw new OperationCanceledException();

        var infoVm = new UserInfoEditorViewModel();
        var infoPage = new UserInfoEditorPage { BindingContext = infoVm };
        result = await ShowDialogAsync(infoPage, infoVm);
        if (!result)
            throw new OperationCanceledException();

        return new SetupData(vm.DatabasePath, vm.ConfigPath);
    }

    private static Task<bool> ShowDialogAsync(ContentPage page, object vm)
    {
        var tcs = new TaskCompletionSource<bool>();
        if (vm is SetupViewModel svm)
        {
            svm.DialogResult += r => { tcs.SetResult(r); };
        }
        else if (vm is SeedOptionsViewModel s)
        {
            s.DialogResult += r => { tcs.SetResult(r); };
        }
        else if (vm is UserInfoEditorViewModel u)
        {
            u.OnOk = _ => tcs.SetResult(true);
            u.OnCancel = () => tcs.SetResult(false);
        }

        Application.Current!.MainPage!.Navigation.PushModalAsync(page);
        return tcs.Task.ContinueWith(async t =>
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
            return t.Result;
        }).Unwrap();
    }
}
