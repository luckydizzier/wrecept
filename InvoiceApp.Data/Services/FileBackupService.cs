using System.IO;
using System.IO.Compression;
using InvoiceApp.Core.Services;

namespace InvoiceApp.Data.Services;

public class FileBackupService : IBackupService
{
    private readonly string _dbPath;
    private readonly string _userInfoPath;
    private readonly string _settingsPath;
    private readonly string _sessionPath;

    public FileBackupService(string dbPath, string userInfoPath, string settingsPath)
    {
        _dbPath = dbPath;
        _userInfoPath = userInfoPath;
        _settingsPath = settingsPath;
        _sessionPath = Path.Combine(Path.GetDirectoryName(settingsPath)!, "session.json");
    }

    public Task BackupAsync(string destinationZipPath, CancellationToken ct = default)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(destinationZipPath)!);
        if (File.Exists(destinationZipPath))
            File.Delete(destinationZipPath);

        using var zip = ZipFile.Open(destinationZipPath, ZipArchiveMode.Create);
        AddFile(zip, _dbPath);
        AddFile(zip, _userInfoPath);
        AddFile(zip, _settingsPath);
        AddFile(zip, _sessionPath);
        return Task.CompletedTask;
    }

    public Task RestoreAsync(string zipPath, CancellationToken ct = default)
    {
        if (!File.Exists(zipPath))
            throw new FileNotFoundException(zipPath);

        using var zip = ZipFile.OpenRead(zipPath);
        foreach (var entry in zip.Entries)
        {
            string? dest = entry.Name switch
            {
                var n when n == Path.GetFileName(_dbPath) => _dbPath,
                var n when n == Path.GetFileName(_userInfoPath) => _userInfoPath,
                var n when n == Path.GetFileName(_settingsPath) => _settingsPath,
                var n when n == Path.GetFileName(_sessionPath) => _sessionPath,
                _ => null
            };
            if (dest != null)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(dest)!);
                entry.ExtractToFile(dest, true);
            }
        }
        return Task.CompletedTask;
    }

    private static void AddFile(ZipArchive zip, string path)
    {
        if (File.Exists(path))
            zip.CreateEntryFromFile(path, Path.GetFileName(path));
    }
}
