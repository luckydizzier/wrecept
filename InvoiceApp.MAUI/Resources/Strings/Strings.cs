namespace InvoiceApp.MAUI.Resources;

using System.Globalization;
using System.Resources;

public static class Strings
{
    private static readonly ResourceManager ResourceManager =
        new("InvoiceApp.MAUI.Resources.Strings.UIStrings", typeof(Strings).Assembly);

    private static string Get(string name) =>
        ResourceManager.GetString(name, CultureInfo.CurrentUICulture) ?? string.Empty;

    public static string StatusBar_DefaultMessage => Get(nameof(StatusBar_DefaultMessage));
    public static string Load_PaymentMethods => Get(nameof(Load_PaymentMethods));
    public static string Load_Suppliers => Get(nameof(Load_Suppliers));
    public static string Load_TaxRates => Get(nameof(Load_TaxRates));
    public static string Load_Products => Get(nameof(Load_Products));
    public static string Load_Units => Get(nameof(Load_Units));
    public static string Load_ProductGroups => Get(nameof(Load_ProductGroups));
    public static string Load_Complete => Get(nameof(Load_Complete));
    public static string InvoiceLine_InvalidQuantity => Get(nameof(InvoiceLine_InvalidQuantity));
    public static string InvoiceLine_InvalidPrice => Get(nameof(InvoiceLine_InvalidPrice));
    public static string InvoiceLine_TaxRequired => Get(nameof(InvoiceLine_TaxRequired));
    public static string InvoiceEditor_ReadOnly => Get(nameof(InvoiceEditor_ReadOnly));
}

