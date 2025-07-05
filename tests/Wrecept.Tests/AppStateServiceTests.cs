using System;
using System.IO;
using System.Threading.Tasks;
using Wrecept.Wpf.Services;
using Wrecept.Wpf.ViewModels;
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
}
