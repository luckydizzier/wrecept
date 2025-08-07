namespace Wrecept.Domain;

public sealed class Product
{
    public Guid Id { get; }
    public string Name { get; }
    public Money Price { get; }
    public TaxRate TaxRate { get; }

    public Product(Guid id, string name, Money price, TaxRate taxRate)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Product id cannot be empty.", nameof(id));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty.", nameof(name));
        Price = price ?? throw new ArgumentNullException(nameof(price));
        TaxRate = taxRate ?? throw new ArgumentNullException(nameof(taxRate));
        Id = id;
        Name = name;
    }
}
