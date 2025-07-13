namespace Wrecept.Core.Models;

public class ChangeLog
{
    public int Id { get; set; }
    public string Entity { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string Operation { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

