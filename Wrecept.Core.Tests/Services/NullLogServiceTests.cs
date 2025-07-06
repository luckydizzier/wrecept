using System;
using System.Threading.Tasks;
using Wrecept.Core.Services;
using Xunit;

namespace Wrecept.Core.Tests.Services;

public class NullLogServiceTests
{
    [Fact]
    public async Task LogError_DoesNotThrow()
    {
        var service = new NullLogService();

        await service.LogError("test", new Exception());
    }
}
