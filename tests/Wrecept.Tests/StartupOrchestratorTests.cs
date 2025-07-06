using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Wrecept.Core.Services;
using Wrecept.Core.Utilities;
using Wrecept.Wpf;
using Xunit;

namespace Wrecept.Tests;

public class StartupOrchestratorTests
{
    private class DummyProgress : IProgress<ProgressReport>
    {
        public readonly List<ProgressReport> Reports = new();
        public void Report(ProgressReport value) => Reports.Add(value);
    }

    [Fact]
    public async Task DatabaseEmptyAsync_ReturnsTrue_ForNewDatabase()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid()+".db");
        App.DbPath = path;
        var orchestrator = new StartupOrchestrator(new NullLogService());

        var empty = await orchestrator.DatabaseEmptyAsync(CancellationToken.None);

        Assert.True(empty);
    }

    [Fact]
    public async Task SeedAsync_ReturnsSeededAndReports()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid()+".db");
        App.DbPath = path;
        var orchestrator = new StartupOrchestrator(new NullLogService());
        var progress = new DummyProgress();

        var status = await orchestrator.SeedAsync(progress, CancellationToken.None,
            1, 1, 1, 1, 1, false);

        Assert.Equal(SeedStatus.Seeded, status);
        Assert.Contains(progress.Reports, r => r.GlobalPercent == 100);
    }
}
