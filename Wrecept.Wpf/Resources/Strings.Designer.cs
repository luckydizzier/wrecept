using System.Resources;
using System.Reflection;

namespace Wrecept.Wpf.Resources;

internal static class Strings
{
    private static readonly ResourceManager _rm = new("Wrecept.Wpf.Resources.Strings", Assembly.GetExecutingAssembly());

    internal static string Stage_ProductViewOpened => _rm.GetString("Stage_ProductViewOpened") ?? string.Empty;
    internal static string Stage_SupplierViewOpened => _rm.GetString("Stage_SupplierViewOpened") ?? string.Empty;
    internal static string Stage_ProductGroupViewOpened => _rm.GetString("Stage_ProductGroupViewOpened") ?? string.Empty;
    internal static string Stage_TaxRateViewOpened => _rm.GetString("Stage_TaxRateViewOpened") ?? string.Empty;
    internal static string Stage_PaymentMethodViewOpened => _rm.GetString("Stage_PaymentMethodViewOpened") ?? string.Empty;
    internal static string Stage_UnitViewOpened => _rm.GetString("Stage_UnitViewOpened") ?? string.Empty;
    internal static string Stage_InvoiceEditorOpened => _rm.GetString("Stage_InvoiceEditorOpened") ?? string.Empty;
    internal static string Stage_AboutOpened => _rm.GetString("Stage_AboutOpened") ?? string.Empty;
    internal static string Stage_UserInfoEditOpened => _rm.GetString("Stage_UserInfoEditOpened") ?? string.Empty;
    internal static string Stage_FunctionNotReady => _rm.GetString("Stage_FunctionNotReady") ?? string.Empty;
    internal static string Stage_DbCheckOk => _rm.GetString("Stage_DbCheckOk") ?? string.Empty;
    internal static string Stage_DbCheckFailed => _rm.GetString("Stage_DbCheckFailed") ?? string.Empty;
    internal static string Stage_LastInvoiceRestored => _rm.GetString("Stage_LastInvoiceRestored") ?? string.Empty;
    internal static string Stage_NoInvoiceToRestore => _rm.GetString("Stage_NoInvoiceToRestore") ?? string.Empty;
    internal static string Stage_BackupSuccess => _rm.GetString("Stage_BackupSuccess") ?? string.Empty;
    internal static string Stage_RestoreSuccess => _rm.GetString("Stage_RestoreSuccess") ?? string.Empty;
    internal static string StatusBar_DefaultMessage => _rm.GetString("StatusBar_DefaultMessage") ?? string.Empty;
    internal static string Load_PaymentMethods => _rm.GetString("Load_PaymentMethods") ?? string.Empty;
    internal static string Load_Suppliers => _rm.GetString("Load_Suppliers") ?? string.Empty;
    internal static string Load_TaxRates => _rm.GetString("Load_TaxRates") ?? string.Empty;
    internal static string Load_Products => _rm.GetString("Load_Products") ?? string.Empty;
    internal static string Load_Units => _rm.GetString("Load_Units") ?? string.Empty;
    internal static string Load_ProductGroups => _rm.GetString("Load_ProductGroups") ?? string.Empty;
    internal static string Load_Complete => _rm.GetString("Load_Complete") ?? string.Empty;
    internal static string InvoiceLine_InvalidQuantity => _rm.GetString("InvoiceLine_InvalidQuantity") ?? string.Empty;
    internal static string InvoiceLine_InvalidPrice => _rm.GetString("InvoiceLine_InvalidPrice") ?? string.Empty;
    internal static string InvoiceLine_TaxRequired => _rm.GetString("InvoiceLine_TaxRequired") ?? string.Empty;
    internal static string InvoiceEditor_ReadOnly => _rm.GetString("InvoiceEditor_ReadOnly") ?? string.Empty;
}
