using Wrecept.Core.Services;
using Wrecept.Storage.Data;
using Wrecept.Storage.Repositories;
using Wrecept.Core.Repositories;

namespace Wrecept.Desktop;

public static class ServiceLocator
{
    public static IInvoiceService InvoiceService { get; }
    public static IProductService ProductService { get; }
    public static IProductGroupRepository ProductGroupRepository { get; }
    public static IProductRepository ProductRepository { get; }
    public static ISupplierRepository SupplierRepository { get; }
    public static ITaxRateRepository TaxRateRepository { get; }
    public static IPaymentMethodRepository PaymentMethodRepository { get; }

    static ServiceLocator()
    {
        var db = new AppDbContext("wrecept.db");
        var invoiceRepo = new InvoiceRepository(db);
        InvoiceService = new InvoiceService(invoiceRepo);
        var productRepo = new ProductRepository(db);
        ProductRepository = productRepo;
        ProductService = new ProductService(productRepo);
        ProductGroupRepository = new ProductGroupRepository(db);
        SupplierRepository = new SupplierRepository(db);
        TaxRateRepository = new TaxRateRepository(db);
        PaymentMethodRepository = new PaymentMethodRepository(db);
    }
}
