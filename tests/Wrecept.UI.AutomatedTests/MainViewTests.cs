using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace Wrecept.UI.AutomatedTests;

[Collection("UI")]
public class MainViewTests : IDisposable
{
    private readonly WindowsDriver<WindowsElement>? _session;
    private readonly Process? _winAppDriver;

    public MainViewTests()
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows),
            "UI tests require Windows");

        // Make WinAppDriver path configurable and check if it exists
        var winAppDriverPath = Environment.GetEnvironmentVariable("WINAPPDRIVER_PATH")
            ?? @"C:\Program Files (x86)\Windows Application Driver\WinAppDriver.exe";
        if (!File.Exists(winAppDriverPath))
        {
            throw new FileNotFoundException($"WinAppDriver.exe not found at '{winAppDriverPath}'. Set WINAPPDRIVER_PATH environment variable to the correct location.");
        }
        _winAppDriver = Process.Start(new ProcessStartInfo
        {
            FileName = winAppDriverPath,
            Arguments = "127.0.0.1 4723"
        });
        if (_winAppDriver is null)
        {
            throw new InvalidOperationException("Failed to start WinAppDriver.");
        }

        var options = new AppiumOptions();
        // Get Wrecept.exe path from environment variable or use default relative path
        var exePath = Environment.GetEnvironmentVariable("WRECEPT_EXE_PATH");
        if (string.IsNullOrWhiteSpace(exePath))
        {
            exePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\wrecept\bin\Debug\net8.0-windows\Wrecept.exe"));
        }
        if (!File.Exists(exePath))
        {
            if (_winAppDriver is { HasExited: false })
            {
                _winAppDriver.Kill();
                _winAppDriver.Dispose();
            }
            throw new FileNotFoundException($"Wrecept.exe not found at '{exePath}'. Set WRECEPT_EXE_PATH environment variable to the correct location.");
        }
        options.AddAdditionalCapability("app", exePath);
        try
        {
            _session = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), options);
        }
        catch
        {
            if (_winAppDriver is { HasExited: false })
            {
                _winAppDriver.Kill();
                _winAppDriver.Dispose();
            }
            throw;
        }
    }

    [SkippableFact(DisplayName = "F2 új sor hozzáadása működik")]
    [Trait("Category", "UI")]
    public void AddInvoiceLine_WithF2_AddsEmptyRow()
    {
        Skip.If(_session is null, "Application session failed to start");
        _session.Keyboard.PressKey(Keys.F2);
        var rows = _session.FindElementsByAccessibilityId("InvoiceLineRow");
        Assert.True(rows.Count > 0);
    }

    public void Dispose()
    {
        try { _session?.Quit(); } catch { }
        if (_winAppDriver is not null)
        {
            if (!_winAppDriver.HasExited)
            {
                _winAppDriver.Kill();
            }
            _winAppDriver.Dispose();
        }
    }
}

[Collection("UI")]
public class MainViewTestsFailures
{
    [SkippableFact]
    [Trait("Category", "UI")]
    public void Constructor_Throws_WhenWinAppDriverMissing()
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows), "UI tests require Windows");

        var original = Environment.GetEnvironmentVariable("WINAPPDRIVER_PATH");
        var missingPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "WinAppDriver.exe");

        var before = Process.GetProcessesByName("WinAppDriver").Length;
        Environment.SetEnvironmentVariable("WINAPPDRIVER_PATH", missingPath);

        try
        {
            var ex = Assert.Throws<FileNotFoundException>(() => new MainViewTests());
            Assert.Contains("WinAppDriver.exe", ex.Message);
            var after = Process.GetProcessesByName("WinAppDriver").Length;
            Assert.Equal(before, after);
        }
        finally
        {
            Environment.SetEnvironmentVariable("WINAPPDRIVER_PATH", original);
        }
    }

    [SkippableFact]
    [Trait("Category", "UI")]
    public void Constructor_Throws_WhenWreceptExeMissing()
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows), "UI tests require Windows");

        var cmdPath = Environment.GetEnvironmentVariable("ComSpec");
        if (string.IsNullOrEmpty(cmdPath) || !File.Exists(cmdPath))
        {
            Skip.If(true, "cmd.exe not found");
        }

        var originalWinApp = Environment.GetEnvironmentVariable("WINAPPDRIVER_PATH");
        var originalExe = Environment.GetEnvironmentVariable("WRECEPT_EXE_PATH");
        var missingExe = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "Wrecept.exe");
        var processName = Path.GetFileNameWithoutExtension(cmdPath);
        var before = Process.GetProcessesByName(processName).Length;

        Environment.SetEnvironmentVariable("WINAPPDRIVER_PATH", cmdPath);
        Environment.SetEnvironmentVariable("WRECEPT_EXE_PATH", missingExe);

        try
        {
            var ex = Assert.Throws<FileNotFoundException>(() => new MainViewTests());
            Assert.Contains("Wrecept.exe", ex.Message);
            var after = Process.GetProcessesByName(processName).Length;
            Assert.Equal(before, after);
        }
        finally
        {
            Environment.SetEnvironmentVariable("WINAPPDRIVER_PATH", originalWinApp);
            Environment.SetEnvironmentVariable("WRECEPT_EXE_PATH", originalExe);
        }
    }

    [SkippableFact]
    [Trait("Category", "UI")]
    public void Constructor_Throws_WhenSessionCreationFails()
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows), "UI tests require Windows");

        var cmdPath = Environment.GetEnvironmentVariable("ComSpec");
        if (string.IsNullOrEmpty(cmdPath) || !File.Exists(cmdPath))
        {
            Skip.If(true, "cmd.exe not found");
        }

        var originalWinApp = Environment.GetEnvironmentVariable("WINAPPDRIVER_PATH");
        var originalExe = Environment.GetEnvironmentVariable("WRECEPT_EXE_PATH");
        var processName = Path.GetFileNameWithoutExtension(cmdPath);
        var before = Process.GetProcessesByName(processName).Length;

        Environment.SetEnvironmentVariable("WINAPPDRIVER_PATH", cmdPath);
        Environment.SetEnvironmentVariable("WRECEPT_EXE_PATH", cmdPath);

        try
        {
            Assert.Throws<WebDriverException>(() => new MainViewTests());
            var after = Process.GetProcessesByName(processName).Length;
            Assert.Equal(before, after);
        }
        finally
        {
            Environment.SetEnvironmentVariable("WINAPPDRIVER_PATH", originalWinApp);
            Environment.SetEnvironmentVariable("WRECEPT_EXE_PATH", originalExe);
        }
    }
}
