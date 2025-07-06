using System.Threading;
using System.Threading.Tasks;
using Wrecept.Core.Services;
using Xunit;

namespace Wrecept.Core.Tests.Services;

public class NullNumberingServiceTests
{
    [Fact]
    public async Task GetNextInvoiceNumberAsync_ReturnsEmptyString()
    {
        var service = new NullNumberingService();

        var result = await service.GetNextInvoiceNumberAsync(CancellationToken.None);

        Assert.Equal(string.Empty, result);
    }
}
