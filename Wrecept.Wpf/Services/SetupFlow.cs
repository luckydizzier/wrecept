using System;
using System.Threading.Tasks;
using System.Windows;
using Wrecept.Core.Entities;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Views;

namespace Wrecept.Wpf.Services;

public class SetupFlow : ISetupFlow
{
    public Task<SetupData> RunAsync(string defaultDb, string defaultCfg)
    {
        var vm = new SetupViewModel(defaultDb, defaultCfg);
        var win = new SetupWindow { DataContext = vm };
        if (win.ShowDialog() != true)
        {
            Application.Current.Shutdown();
            Environment.Exit(0);
        }

        var infoVm = new UserInfoEditorViewModel();
        var infoWin = new UserInfoWindow { DataContext = infoVm };
        infoVm.OnOk = _ => { infoWin.DialogResult = true; infoWin.Close(); };
        infoVm.OnCancel = () => { infoWin.DialogResult = false; infoWin.Close(); };
        if (infoWin.ShowDialog() != true)
        {
            Application.Current.Shutdown();
            Environment.Exit(0);
        }

        var info = new UserInfo
        {
            CompanyName = infoVm.CompanyName,
            Address = infoVm.Address,
            Phone = infoVm.Phone,
            Email = infoVm.Email,
            TaxNumber = infoVm.TaxNumber,
            BankAccount = infoVm.BankAccount
        };

        return Task.FromResult(new SetupData(vm.DatabasePath, vm.ConfigPath, info));
    }
}
