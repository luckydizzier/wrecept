namespace InvoiceApp.Core.Services;

public interface IDatabaseInitializer
{
    Task InitializeAsync(CancellationToken ct = default);
}
