namespace Wrecept.Core.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Net { get; set; }
    public decimal Gross { get; set; }
    public Guid TaxRateId { get; set; }
    public TaxRate? TaxRate { get; set; }
    public Guid UnitId { get; set; }
    public Unit? Unit { get; set; }
    public Guid ProductGroupId { get; set; }
    public ProductGroup? ProductGroup { get; set; }
    public bool IsArchived { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
