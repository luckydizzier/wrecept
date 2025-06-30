namespace Wrecept.Core.Models;

public class PaymentMethod
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DueInDays { get; set; } = 0;
    public bool IsArchived { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
