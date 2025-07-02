using System;
using System.Threading;
using System.Threading.Tasks;
using Wrecept.Core.Utilities;
using Wrecept.Storage.Data;
using Wrecept.Core.Services;

namespace Wrecept.Wpf;

public class StartupOrchestrator
{
    private readonly ILogService _log;

    public StartupOrchestrator(ILogService log)
    {
        _log = log;
    }

    public Task<bool> DatabaseEmptyAsync(CancellationToken ct)
        => DataSeeder.IsDatabaseEmptyAsync(App.DbPath, _log, ct);

    public async Task<SeedStatus> SeedAsync(IProgress<ProgressReport> progress, CancellationToken ct)
    {
        progress.Report(new ProgressReport { GlobalPercent = 10, Message = "Mintaszámlák létrehozása..." });
        var status = await DataSeeder.SeedSampleDataAsync(App.DbPath, _log, progress, ct);
        progress.Report(new ProgressReport { GlobalPercent = 100, SubtaskPercent = 100, Message = status.ToString() });
        return status;
    }
}
