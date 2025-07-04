using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text.Json;
using Wrecept.Core.Entities;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Wrecept.UiTests;

[TestClass]
public class StartupWindowTests
{
    private const string ExePath = @"C:\Users\tankoferenc\source\repos\luckydizzier\wrecept\Wrecept.Wpf\bin\Debug\net8.0-windows\Wrecept.Wpf.exe";
    private const string WinAppDriverUrl = "http://127.0.0.1:4723";

    private static void PrepareSettings(bool firstRun)
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

    private static WindowsDriver<WindowsElement> LaunchApp()
    {
        var options = new AppiumOptions();
        options.AddAdditionalCapability("app", ExePath);

        var driver = new WindowsDriver<WindowsElement>(new Uri(WinAppDriverUrl), options);
        DismissFirstLaunchDialogs(driver);
        return driver;
    }

    private static void DismissFirstLaunchDialogs(WindowsDriver<WindowsElement> driver)
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
                catch (OpenQA.Selenium.WebDriverException)
                {
                    // No 'Igen' button, likely a setup window
                }
            }
        }
    }

    [TestMethod]
    public void Application_Launches_And_Closes()
    {
        PrepareSettings(firstRun: false);
        using var driver = LaunchApp();
        Assert.AreEqual("Wrecept", driver.Title);
        driver.Close();
    }

    [TestMethod]
    public void SeedOptions_Cancel_OpensMainWindow()
    {
        PrepareSettings(firstRun: false);
        using var driver = LaunchApp();

        var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(5));
        var optionsWindow = wait.Until(d => ((WindowsDriver<WindowsElement>)d)
            .FindElementByName("Mintaszámok"));
        optionsWindow.FindElementByName("Mégse").Click();

        var mainWindow = wait.Until(d => ((WindowsDriver<WindowsElement>)d)
            .FindElementByName("Wrecept"));
        Assert.IsNotNull(mainWindow);
        driver.Close();
    }

    [TestMethod]
    public void SeedOptions_Ok_ShowsStartupWindow()
    {
        PrepareSettings(firstRun: false);
        using var driver = LaunchApp();

        var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(5));
        var optionsWindow = wait.Until(d => ((WindowsDriver<WindowsElement>)d)
            .FindElementByName("Mintaszámok"));
        optionsWindow.FindElementByName("OK").Click();

        var startupWindow = wait.Until(d => ((WindowsDriver<WindowsElement>)d)
            .FindElementByName("Indulás"));
        Assert.IsNotNull(startupWindow);

        driver.Close();
    }

    [TestMethod]
    public void UserInfoEditor_FillFields_Confirms()
    {
        PrepareSettings(firstRun: true);
        using var driver = LaunchApp();

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        var setupWindow = wait.Until(d => ((WindowsDriver<WindowsElement>)d)
            .FindElementByName("Első indítás"));
        setupWindow.FindElementByName("OK").Click();

        var editorWindow = wait.Until(d => ((WindowsDriver<WindowsElement>)d)
            .FindElementByName("Tulajdonosi adatok"));
        wait.Until(d => ((WindowsDriver<WindowsElement>)d)
            .FindElementByAccessibilityId("CompanyNameBox")).SendKeys("Teszt Kft.");
        driver.FindElementByAccessibilityId("AddressBox").SendKeys("Siklós, Fő utca 1.");
        driver.FindElementByAccessibilityId("PhoneBox").SendKeys("1111");
        driver.FindElementByAccessibilityId("EmailBox").SendKeys("teszt@example.hu");
        driver.FindElementByAccessibilityId("TaxNumberBox").SendKeys("123");
        driver.FindElementByAccessibilityId("BankAccountBox").SendKeys("111");
        new Actions(driver).SendKeys(OpenQA.Selenium.Keys.Enter).Perform();

        wait.Until(d => ((WindowsDriver<WindowsElement>)d)
            .FindElementByName("Wrecept"));
        driver.Close();
    }
}
