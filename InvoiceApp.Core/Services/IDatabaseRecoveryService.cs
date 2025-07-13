namespace InvoiceApp.Core.Services;

public interface IDatabaseRecoveryService
{
    Task CheckAndRecoverAsync(CancellationToken ct = default);
}

