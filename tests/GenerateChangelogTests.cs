using System;
using System.Diagnostics;
using System.IO;
using Xunit;

public class GenerateChangelogTests
{
    [Fact]
    public void Script_Generates_Changelog()
    {
        var temp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(Path.Combine(temp, "docs", "progress"));
        File.WriteAllText(Path.Combine(temp, "docs", "progress", "2025-01-01_test.md"), "* first");
        File.WriteAllText(Path.Combine(temp, "docs", "progress", "2025-01-02_test.md"), "* second");

        var psi = new ProcessStartInfo("python3", Path.GetFullPath("tools/generate_changelog.py"))
        {
            WorkingDirectory = temp,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        using var proc = Process.Start(psi)!;
        proc.WaitForExit();
        var content = File.ReadAllText(Path.Combine(temp, "CHANGELOG.md"));
        Assert.Contains("## 2025-01-01", content);
        Assert.Contains("- first", content);
        Assert.Contains("- second", content);
        Directory.Delete(temp, true);
    }
}
