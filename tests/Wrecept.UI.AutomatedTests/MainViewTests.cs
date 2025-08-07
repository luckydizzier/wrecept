using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using OpenQA.Selenium;
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

        var options = new OpenQA.Selenium.Appium.AppiumOptions();
        // Get Wrecept.exe path from environment variable or use default relative path
        var exePath = Environment.GetEnvironmentVariable("WRECEPT_EXE_PATH");
        if (string.IsNullOrWhiteSpace(exePath))
        {
            exePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\wrecept\bin\Debug\net8.0-windows\Wrecept.exe"));
        }
        options.AddAdditionalCapability("app", exePath);
        _session = new WindowsDriver(new Uri("http://127.0.0.1:4723"), options);
    }

    [SkippableFact(DisplayName = "F2 új sor hozzáadása működik")]
    [Trait("Category", "UI")]
    public void AddInvoiceLine_WithF2_AddsEmptyRow()
    {
        _session!.Keyboard.PressKey(Keys.F2);
        var rows = _session.FindElementsByAccessibilityId("InvoiceLineRow");
        Assert.True(rows.Count > 0);
    }

    public void Dispose()
    {
        try { _session?.Quit(); } catch { }
        if (_winAppDriver is { HasExited: false })
        {
            _winAppDriver.Kill();
            _winAppDriver.Dispose();
        }
    }
}
