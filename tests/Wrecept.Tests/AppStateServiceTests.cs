using System;
using System.IO;
using System.Threading.Tasks;
using InvoiceApp.MAUI.Services;
using InvoiceApp.MAUI.ViewModels;
using InvoiceApp.Core.Enums;
using Xunit;

namespace Wrecept.Tests;

public class AppStateServiceTests
{
    [Fact]
    public async Task SaveAndLoad_RoundTrip()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        var svc = new AppStateService(path)
        {
            LastView = StageMenuAction.EditProducts,
            CurrentInvoiceId = 5
        };
        await svc.SaveAsync();

        var svc2 = new AppStateService(path);
        await svc2.LoadAsync();

        Assert.Equal(StageMenuAction.EditProducts, svc2.LastView);
        Assert.Equal(5, svc2.CurrentInvoiceId);
    }

    [Fact]
    public async Task LoadAsync_IgnoresMissingFile()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        var svc = new AppStateService(path);

        await svc.LoadAsync();

        Assert.Equal(StageMenuAction.InboundDeliveryNotes, svc.LastView);
        Assert.Null(svc.CurrentInvoiceId);
    }

    [Fact]
    public async Task LoadAsync_IgnoresInvalidJson()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        await File.WriteAllTextAsync(path, "{invalid json}");
        var svc = new AppStateService(path);

        await svc.LoadAsync();

        Assert.Equal(StageMenuAction.InboundDeliveryNotes, svc.LastView);
        Assert.Null(svc.CurrentInvoiceId);
    }
}
