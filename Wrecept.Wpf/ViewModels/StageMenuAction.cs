namespace Wrecept.Wpf.ViewModels
{
    public enum StageMenuAction
    {
        // Számlák menu
        InboundDeliveryNotes,
        UpdateInboundInvoices,
        
        // Törzsek menu
        EditProducts,
        EditProductGroups,
        EditSuppliers,
        EditVatKeys,
        EditPaymentMethods,
        EditUnits,
        
        // Listák menu
        ListProducts,
        ListSuppliers,
        ListInvoices,
        InventoryCard,
        
        // Szerviz menu
        CheckFiles,
        AfterPowerOutage,
        ScreenSettings,
        PrinterSettings,
        EditUserInfo,
        
        // Névjegy menu
        UserInfo,
        
        // Vége menu
        ExitApplication
    }
}