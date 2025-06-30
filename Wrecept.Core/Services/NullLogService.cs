namespace Wrecept.Core.Services;

public class NullLogService : ILogService
{
    public Task LogError(string message, Exception ex) => Task.CompletedTask;
}
