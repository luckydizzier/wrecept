using InvoiceApp.Core.Enums;
using InvoiceApp.Core.Services;
using InvoiceApp.Core.Utilities;

namespace InvoiceApp.MAUI;

public class StartupOrchestrator
{
    private readonly ILogService _log;
    private readonly IDatabaseRecoveryService _recovery;

    public StartupOrchestrator(ILogService log, IDatabaseRecoveryService recovery)
    {
        _log = log;
        _recovery = recovery;
    }

    public Task<bool> DatabaseEmptyAsync(CancellationToken ct)
        => Task.FromResult(true);

    public Task<SeedStatus> SeedAsync(
        IProgress<ProgressReport> progress,
        CancellationToken ct,
        int supplierCount,
        int productCount,
        int invoiceCount,
        int minItems,
        int maxItems,
        bool slow)
        => Task.FromResult(SeedStatus.None);
}
