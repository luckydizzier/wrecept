using System;
using System.IO;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using Wrecept.Core.Entities;
using System.Text.Json;

namespace Wrecept.UiTests;

internal static class TestHelper
{
    private const string WinAppDriverUrl = "http://127.0.0.1:4723";

    internal static string ExePath =>
        Path.GetFullPath(Path.Combine(AppContext.BaseDirectory,
            "../../../../Wrecept.Wpf/bin/Debug/net8.0-windows/Wrecept.Wpf.exe"));

    internal static void PrepareSettings(bool firstRun)
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var dataDir = Path.Combine(appData, "Wrecept");
        Directory.CreateDirectory(dataDir);
        var settingsPath = Path.Combine(dataDir, "settings.json");
        if (firstRun)
        {
            if (File.Exists(settingsPath))
                File.Delete(settingsPath);
        }
        else
        {
            var settings = new AppSettings
            {
                DatabasePath = Path.Combine(dataDir, "test.db"),
                UserInfoPath = Path.Combine(dataDir, "user.json")
            };
            File.WriteAllText(settingsPath, JsonSerializer.Serialize(settings));
        }
    }

    internal static WindowsDriver<WindowsElement> LaunchApp(bool waitMainWindow = true)
    {
        var options = new AppiumOptions();
        options.AddAdditionalCapability("app", ExePath);
        var driver = new WindowsDriver<WindowsElement>(new Uri(WinAppDriverUrl), options);
        DismissFirstLaunchDialogs(driver);
        if (waitMainWindow)
            WaitForMainWindow(driver);
        return driver;
    }

    internal static void DismissFirstLaunchDialogs(WindowsDriver<WindowsElement> driver)
    {
        foreach (var title in new[] { "Első indítás", "Megerősítés" })
        {
            var dialogs = driver.FindElementsByName(title);
            if (dialogs.Count > 0)
            {
                try
                {
                    dialogs[0].FindElementByName("Igen").Click();
                }
                catch (WebDriverException)
                {
                    // optional confirmation window
                }
            }
        }
    }

    internal static void WaitForMainWindow(WindowsDriver<WindowsElement> driver)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        wait.Until(d => d.Title == "Wrecept");
    }
}
