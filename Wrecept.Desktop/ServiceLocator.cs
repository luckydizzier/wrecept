using Wrecept.Core.Services;
using Wrecept.Storage.Data;
using Wrecept.Storage.Repositories;

namespace Wrecept.Desktop;

public static class ServiceLocator
{
    public static IInvoiceService InvoiceService { get; }

    static ServiceLocator()
    {
        var db = new AppDbContext("wrecept.db");
        var invoiceRepo = new InvoiceRepository(db);
        InvoiceService = new InvoiceService(invoiceRepo);
    }
}
