using System;
using System.IO;
using System.Threading.Tasks;
using Wrecept.Storage.Services;
using Xunit;

namespace Wrecept.Core.Tests.Services;

public class NumberingServiceTests : IDisposable
{
    private readonly string _file;

    public NumberingServiceTests()
    {
        _file = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
    }

    [Fact]
    public async Task GeneratesSequentialNumbers()
    {
        var svc = new NumberingService(_file);
        var first = await svc.GetNextInvoiceNumberAsync();
        var second = await svc.GetNextInvoiceNumberAsync();

        Assert.Equal("INV1", first);
        Assert.Equal("INV2", second);
    }

    public void Dispose()
    {
        if (File.Exists(_file))
            File.Delete(_file);
    }
}
