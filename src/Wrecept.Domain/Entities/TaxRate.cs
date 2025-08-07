namespace Wrecept.Domain;

public sealed class TaxRate
{
    public Guid Id { get; }
    public string Name { get; }
    public decimal Rate { get; }

    public TaxRate(Guid id, string name, decimal rate)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Tax rate id cannot be empty.", nameof(id));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tax rate name cannot be empty.", nameof(name));
        if (rate < 0m || rate > 1m)
            throw new ArgumentOutOfRangeException(nameof(rate), "Tax rate must be between 0 and 1.");

        Id = id;
        Name = name;
        Rate = rate;
    }
}
