namespace Wrecept.Domain;

public sealed class InvoiceLine
{
    public Product Product { get; }
    public int Quantity { get; }
    public Money UnitPrice { get; }
    public Money Total => UnitPrice.Multiply(Quantity);

    public InvoiceLine(Product product, int quantity, Money unitPrice)
    {
        Product = product ?? throw new ArgumentNullException(nameof(product));
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        UnitPrice = unitPrice ?? throw new ArgumentNullException(nameof(unitPrice));

        Quantity = quantity;
    }
}
