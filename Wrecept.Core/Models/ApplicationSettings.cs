namespace Wrecept.Core.Models;

public class ApplicationSettings
{
    public string DatabasePath { get; set; } = "Data/wrecept.db";
    public string Theme { get; set; } = "Light";
    public string Language { get; set; } = "en";
}
