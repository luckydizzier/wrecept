using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;

namespace Wrecept.UI.AutomatedTests;

[Collection("UI")]
public class MainViewTests : IDisposable
{
    private readonly dynamic? _session;
    private readonly Process? _winAppDriver;

    public MainViewTests()
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows),
            "UI tests require Windows");

        _winAppDriver = Process.Start(new ProcessStartInfo
        {
            FileName = @"C:\\Program Files (x86)\\Windows Application Driver\\WinAppDriver.exe",
            Arguments = "127.0.0.1 4723"
        });

        dynamic options = new OpenQA.Selenium.Appium.AppiumOptions();
        options.AddAdditionalCapability("app", @"C:\\wrecept\\bin\\Debug\\net8.0-windows\\Wrecept.exe");
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
