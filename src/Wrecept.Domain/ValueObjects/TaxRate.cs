namespace Wrecept.Domain;

public sealed record TaxRate
{
    public decimal Rate { get; }

    public TaxRate(decimal rate)
    {
        if (rate < 0m || rate > 1m)
            throw new ArgumentOutOfRangeException(nameof(rate), "Tax rate must be between 0 and 1.");
        Rate = rate;
    }

    public Money ApplyTo(Money netAmount)
    {
        if (netAmount is null) throw new ArgumentNullException(nameof(netAmount));
        return new Money(netAmount.Currency, netAmount.Amount * Rate);
    }
}
