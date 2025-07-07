using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Views;

namespace Wrecept.Tests;

public class PaymentMethodMasterViewTests
{
    private static void EnsureApp()
    {
        if (Application.Current == null)
            new Application();
        Application.Current.Resources["RetroDataGridStyle"] = new Style(typeof(DataGrid));
        Application.Current.Resources["RetroDataGridRowStyle"] = new Style(typeof(DataGridRow));
        Application.Current.Resources["BooleanToRowDetailsConverter"] = new Wrecept.Wpf.Converters.BooleanToRowDetailsConverter();
    }

    private class FakeService : IPaymentMethodService
    {
        public Task<List<PaymentMethod>> GetAllAsync(CancellationToken ct = default) => Task.FromResult(new List<PaymentMethod>());
        public Task<List<PaymentMethod>> GetActiveAsync(CancellationToken ct = default) => Task.FromResult(new List<PaymentMethod>());
        public Task<Guid> AddAsync(PaymentMethod method, CancellationToken ct = default) => Task.FromResult(Guid.NewGuid());
        public Task UpdateAsync(PaymentMethod method, CancellationToken ct = default) => Task.CompletedTask;
    }

    [StaFact]
    public void Constructor_NoArgs_UsesAppProvider()
    {
        EnsureApp();
        var services = new ServiceCollection();
        services.AddTransient<IPaymentMethodService, FakeService>();
        services.AddTransient(sp => new PaymentMethodMasterViewModel(sp.GetRequiredService<IPaymentMethodService>(), new AppStateService(Path.GetTempFileName())));
        App.Services = services.BuildServiceProvider();

        var view = new PaymentMethodMasterView();

        Assert.IsType<PaymentMethodMasterViewModel>(view.DataContext);
    }

    [StaFact]
    public void Constructor_WithViewModel_SetsDataContext()
    {
        EnsureApp();
        var vm = new PaymentMethodMasterViewModel(new FakeService(), new AppStateService(Path.GetTempFileName()));

        var view = new PaymentMethodMasterView(vm);

        Assert.Same(vm, view.DataContext);
    }
}
