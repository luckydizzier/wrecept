using System;
using System.IO;
using System.Threading.Tasks;
using Wrecept.Storage.Services;
using Xunit;

namespace Wrecept.Tests;

public class FileBackupServiceTests
{
    [Fact]
    public async Task BackupAndRestore_RoundTrip()
    {
        var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(dir);
        var db = Path.Combine(dir, "app.db");
        var user = Path.Combine(dir, "user.json");
        var settings = Path.Combine(dir, "settings.json");
        var session = Path.Combine(dir, "session.json");
        await File.WriteAllTextAsync(db, "db");
        await File.WriteAllTextAsync(user, "user");
        await File.WriteAllTextAsync(settings, "settings");
        await File.WriteAllTextAsync(session, "session");

        var svc = new FileBackupService(db, user, settings);
        var zip = Path.Combine(dir, "backup.zip");
        await svc.BackupAsync(zip);

        File.Delete(db);
        File.Delete(user);
        File.Delete(settings);
        File.Delete(session);

        await svc.RestoreAsync(zip);

        Assert.True(File.Exists(db));
        Assert.True(File.Exists(user));
        Assert.True(File.Exists(settings));
        Assert.True(File.Exists(session));

        Directory.Delete(dir, true);
    }

    [Fact]
    public async Task RestoreAsync_ThrowsIfZipMissing()
    {
        var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(dir);
        var db = Path.Combine(dir, "app.db");
        var user = Path.Combine(dir, "user.json");
        var settings = Path.Combine(dir, "settings.json");
        var svc = new FileBackupService(db, user, settings);
        var zip = Path.Combine(dir, "missing.zip");

        await Assert.ThrowsAsync<FileNotFoundException>(() => svc.RestoreAsync(zip));

        Directory.Delete(dir, true);
    }
}
