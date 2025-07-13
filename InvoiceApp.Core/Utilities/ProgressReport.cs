namespace InvoiceApp.Core.Utilities;

public class ProgressReport
{
    public int GlobalPercent { get; set; }
    public int SubtaskPercent { get; set; }
    public string Message { get; set; } = string.Empty;
}
