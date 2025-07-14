using System.Threading;
using System.Threading.Tasks;
using InvoiceApp.Core.Services;
using Xunit;

namespace InvoiceApp.Core.Tests.Services;

public class NullNumberingServiceTests
{
    [Fact]
    public async Task GetNextInvoiceNumberAsync_ReturnsEmptyString()
    {
        var service = new NullNumberingService();

        var result = await service.GetNextInvoiceNumberAsync(0, CancellationToken.None);

        Assert.Equal(string.Empty, result);
    }
}
