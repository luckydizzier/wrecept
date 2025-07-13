namespace InvoiceApp.MAUI.Services;

public record SetupData(string DatabasePath, string ConfigPath);

public interface ISetupFlow
{
    Task<SetupData> RunAsync(string defaultDb, string defaultCfg);
}
