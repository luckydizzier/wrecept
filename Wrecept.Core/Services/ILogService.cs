namespace Wrecept.Core.Services;

public interface ILogService
{
    Task LogError(string message, Exception ex);
}
