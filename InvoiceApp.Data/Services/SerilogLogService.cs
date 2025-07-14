using System;
using System.IO;
using Serilog;
using Serilog.Formatting.Json;
using InvoiceApp.Core.Services;

namespace InvoiceApp.Data.Services;

public class SerilogLogService : ILogService
{
    private readonly ILogger _logger;

    public SerilogLogService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var logDir = Path.Combine(appData, "InvoiceApp", "logs");
        try
        {
            Directory.CreateDirectory(logDir);
            var path = Path.Combine(logDir, "log-.json");
            _logger = new LoggerConfiguration()
                .WriteTo.File(new JsonFormatter(), path,
                    rollingInterval: RollingInterval.Day,
                    fileSizeLimitBytes: 5_000_000,
                    rollOnFileSizeLimit: true,
                    retainedFileCountLimit: 5,
                    shared: true)
                .CreateLogger();
        }
        catch
        {
            _logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }
    }

    public Task LogError(string message, Exception ex)
    {
        _logger.Error(ex, message);
        return Task.CompletedTask;
    }
}
