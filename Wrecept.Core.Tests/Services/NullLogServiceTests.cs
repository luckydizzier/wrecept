using System;
using System.Threading.Tasks;
using InvoiceApp.Core.Services;
using Xunit;

namespace InvoiceApp.Core.Tests.Services;

public class NullLogServiceTests
{
    [Fact]
    public async Task LogError_DoesNotThrow()
    {
        var service = new NullLogService();

        await service.LogError("test", new Exception());
    }
}
