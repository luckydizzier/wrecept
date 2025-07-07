using System.Threading.Tasks;
using Wrecept.Core.Services;
using Xunit;

namespace Wrecept.Core.Tests;

public class NullNumberingServiceTests
{
    [Fact]
    public async Task GetNextInvoiceNumberAsync_ReturnsEmpty()
    {
        var service = new NullNumberingService();

        var result = await service.GetNextInvoiceNumberAsync();

        Assert.Equal(string.Empty, result);
    }
}
