using System;
using System.Threading.Tasks;
using Wrecept.Core.Services;
using Xunit;

namespace Wrecept.Core.Tests;

public class NullLogServiceTests
{
    [Fact]
    public async Task LogError_DoesNothing()
    {
        var service = new NullLogService();

        var task = service.LogError("err", new Exception());

        await task;
        Assert.True(task.IsCompleted);
    }
}
