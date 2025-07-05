namespace Wrecept.Core.Models;

public class Invoice
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public int SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    public Guid PaymentMethodId { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public DateOnly DueDate { get; set; }
    public bool IsGross { get; set; } = false;
    public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
    public bool IsArchived { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
