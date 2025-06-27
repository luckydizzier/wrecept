namespace Wrecept.Core.Models;

public class Invoice
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Net { get; set; }
    public decimal Gross { get; set; }
}
