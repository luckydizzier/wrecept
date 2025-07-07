using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Wrecept.Core.Entities;
using Wrecept.Wpf.Services;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Views;
using Xunit;

namespace Wrecept.Tests;

public class SetupFlowTests
{
    private static void EnsureApp()
    {
        if (Application.Current == null)
            new Application();
    }

    [StaFact]
    public async Task RunAsync_ReturnsDialogData()
    {
        EnsureApp();
        const string db = "test.db";
        const string cfg = "cfg.json";

        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
        {
            var setup = Application.Current.Windows.OfType<SetupWindow>().First();
            if (setup.DataContext is SetupViewModel svm)
            {
                svm.DatabasePath = db;
                svm.ConfigPath = cfg;
                svm.OkCommand.Execute(setup);
            }
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                var info = Application.Current.Windows.OfType<UserInfoWindow>().First();
                if (info.DataContext is UserInfoEditorViewModel ivm)
                {
                    ivm.CompanyName = "C";
                    ivm.Address = "A";
                    ivm.Phone = "P";
                    ivm.Email = "e@x.hu";
                    ivm.TaxNumber = "T";
                    ivm.BankAccount = "B";
                    ivm.OnOk?.Invoke(ivm);
                }
            }), DispatcherPriority.Background);
        }), DispatcherPriority.Background);

        var flow = new SetupFlow();
        var result = await flow.RunAsync("db", "cfg");

        Assert.Equal(db, result.DatabasePath);
        Assert.Equal(cfg, result.ConfigPath);
        Assert.Equal("C", result.Info.CompanyName);
        Assert.Equal("A", result.Info.Address);
        Assert.Equal("P", result.Info.Phone);
        Assert.Equal("e@x.hu", result.Info.Email);
        Assert.Equal("T", result.Info.TaxNumber);
        Assert.Equal("B", result.Info.BankAccount);
    }
}
