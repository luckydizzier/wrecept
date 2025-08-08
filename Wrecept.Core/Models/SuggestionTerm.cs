namespace Wrecept.Core.Models;

public class SuggestionTerm
{
    public int Id { get; set; }
    public string Term { get; set; } = string.Empty;
    public int Frequency { get; set; }
    public DateTime LastUsedUtc { get; set; }
}
