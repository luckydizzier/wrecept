namespace Wrecept.Domain;

public sealed record Money
{
    public string Currency { get; }
    public decimal Amount { get; }

    public Money(string currency, decimal amount)
    {
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency must not be empty.", nameof(currency));
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount cannot be negative.");

        Currency = currency;
        Amount = amount;
    }

    public Money Add(Money other)
    {
        if (other is null) throw new ArgumentNullException(nameof(other));
        if (!string.Equals(Currency, other.Currency, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Cannot add money with different currencies.");
        return new Money(Currency, Amount + other.Amount);
    }

    public Money Multiply(int factor)
    {
        if (factor < 0) throw new ArgumentOutOfRangeException(nameof(factor), "Factor cannot be negative.");
        return new Money(Currency, Amount * factor);
    }

    public static Money Zero(string currency) => new(currency, 0m);
}
