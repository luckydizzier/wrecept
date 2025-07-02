namespace Wrecept.Core.Entities;

public class AppSettings
{
    public ScreenMode ScreenMode { get; set; } = ScreenMode.Large;
    public string DatabasePath { get; set; } = string.Empty;
    public string UserInfoPath { get; set; } = string.Empty;
}
