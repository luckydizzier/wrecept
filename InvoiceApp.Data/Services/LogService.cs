using System;
using System.IO;
using InvoiceApp.Core.Services;

namespace InvoiceApp.Data.Services;

public class LogService : ILogService
{
    private readonly string _logDir;

    public LogService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        _logDir = Path.Combine(appData, "Wrecept", "logs");
    }

    public async Task LogError(string message, Exception ex)
    {
        try
        {
            Directory.CreateDirectory(_logDir);
            var path = Path.Combine(_logDir, $"{DateTime.UtcNow:yyyyMMdd}.log");
            var entry = $"{DateTime.UtcNow:u} {message} {ex}\n";
            await File.AppendAllTextAsync(path, entry);
        }
        catch (Exception ioEx)
        {
            Console.Error.WriteLine($"{DateTime.UtcNow:u} {message} {ioEx}");
        }
    }
}
