namespace Wrecept.Core.Services;

public interface IDbHealthService
{
    Task<bool> CheckAsync(CancellationToken ct = default);
}
