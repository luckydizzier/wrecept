namespace Wrecept.Domain;

public sealed class Invoice
{
    public Guid Id { get; }
    public Customer Customer { get; }
    private readonly List<InvoiceLine> _lines;
    public IReadOnlyCollection<InvoiceLine> Lines => _lines.AsReadOnly();
    public Money Total { get; }

    public Invoice(Guid id, Customer customer, IEnumerable<InvoiceLine> lines)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Invoice id cannot be empty.", nameof(id));
        Customer = customer ?? throw new ArgumentNullException(nameof(customer));
        if (lines is null)
            throw new ArgumentNullException(nameof(lines));

        _lines = lines.ToList();
        if (_lines.Count == 0)
            throw new ArgumentException("Invoice must contain at least one line.", nameof(lines));

        var currency = _lines[0].Total.Currency;
        var total = Money.Zero(currency);
        foreach (var line in _lines)
        {
            if (line.Total.Currency != currency)
                throw new InvalidOperationException("All lines must use the same currency.");
            total = total.Add(line.Total);
        }

        Total = total;
        Id = id;
    }
}
