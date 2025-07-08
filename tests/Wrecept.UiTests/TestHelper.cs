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
        @"C:\\Users\\tankoferenc\\source\\repos\\luckydizzier\\wrecept\\Wrecept.Wpf\\bin\\Debug\\net8.0-windows\\Wrecept.Wpf.exe";

    internal static void PrepareSettings(bool firstRun)
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var dataDir = Path.Combine(appData, "Wrecept");
        Directory.CreateDirectory(dataDir);
        var settingsPath = Path.Combine(dataDir, "settings.json");
        var dbPath = Path.Combine(dataDir, "test.db");
        var userPath = Path.Combine(dataDir, "user.json");

        if (File.Exists(dbPath))
            File.Delete(dbPath);

        if (firstRun)
        {
            if (File.Exists(settingsPath))
                File.Delete(settingsPath);
            if (File.Exists(userPath))
                File.Delete(userPath);
        }
        else
        {
            var settings = new AppSettings
            {
                DatabasePath = dbPath,
                UserInfoPath = userPath
            };
            File.WriteAllText(settingsPath, JsonSerializer.Serialize(settings));
        }
    }

    internal static WindowsDriver<WindowsElement> LaunchApp(bool waitMainWindow = true)
    {
        var options = new AppiumOptions();
        options.AddAdditionalCapability("app", ExePath);

        var driver = new WindowsDriver<WindowsElement>(new Uri(WinAppDriverUrl), options);

        try
        {
            WaitForUiReady(driver);

            switch (driver.Title)
            {
                case "Első indítás":
                    RunFirstLaunchSetup(driver);
                    break;
                case "Wrecept":
                    ValidateMainWindowLoaded(driver);
                    break;
                default:
                    Assert.Fail($"Váratlan ablakcím: {driver.Title}");
                    break;
            }

            if (waitMainWindow)
                WaitForMainWindow(driver);

            return driver;
        }
        catch
        {
            SaveScreenshot(driver);
            driver.Close();
            throw;
        }
    }

    internal static WindowsDriver<WindowsElement> LaunchAppRaw()
    {
        var options = new AppiumOptions();
        options.AddAdditionalCapability("app", ExePath);

        var driver = new WindowsDriver<WindowsElement>(new Uri(WinAppDriverUrl), options);

        try
        {
            WaitForUiReady(driver);
            return driver;
        }
        catch
        {
            SaveScreenshot(driver);
            driver.Close();
            throw;
        }
    }

    internal static void DismissFirstLaunchDialogs(WindowsDriver<WindowsElement> driver)
    {
        foreach (var title in new[] { "Első indítás", "Megerősítés" })
        {
            var dialogs = driver.FindElements(By.Name(title));
            if (dialogs.Count > 0)
            {
                try
                {
                    dialogs[0].FindElement(By.Name("Igen")).Click();
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

    internal static void WaitForUiReady(WindowsDriver<WindowsElement> driver)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        wait.Until(d => d.PageSource.Length > 0);
    }

    internal static WindowsElement WaitForElementById(WindowsDriver<WindowsElement> driver, string id)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        return wait.Until(_ => driver.FindElementByAccessibilityId(id));
    }

    internal static void HandleConfirmationDialog(WindowsDriver<WindowsElement> driver)
    {
        if (driver.PageSource.Contains("Megerősítés"))
        {
            var dialog = driver.FindElement(By.Name("Megerősítés"));
            dialog.FindElement(By.Name("Igen")).Click();
        }
    }

    internal static void RunFirstLaunchSetup(WindowsDriver<WindowsElement> driver)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        var setupWindow = wait.Until(_ => driver.FindElement(By.Name("Első indítás")));
        setupWindow.FindElement(By.Name("OK")).Click();

        var companyBox = WaitForElementById(driver, "CompanyNameBox");
        companyBox.SendKeys("Teszt Kft.");
        driver.FindElementByAccessibilityId("AddressBox").SendKeys("Teszt cím");
        driver.FindElementByAccessibilityId("PhoneBox").SendKeys("123");
        driver.FindElementByAccessibilityId("EmailBox").SendKeys("test@example.hu");
        driver.FindElementByAccessibilityId("TaxNumberBox").SendKeys("1111");
        driver.FindElementByAccessibilityId("BankAccountBox").SendKeys("0000");

        driver.FindElement(By.Name("OK")).Click();
        HandleConfirmationDialog(driver);
    }

    internal static void ValidateMainWindowLoaded(WindowsDriver<WindowsElement> driver)
    {
        var menu = WaitForElementById(driver, "MainMenuBar");
        Assert.IsNotNull(menu);
    }

    internal static void SaveScreenshot(WindowsDriver<WindowsElement> driver)
    {
        try
        {
            var file = $"error_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            driver.GetScreenshot().SaveAsFile(file);
        }
        catch
        {
            // ignore screenshot failures
        }
    }
}
