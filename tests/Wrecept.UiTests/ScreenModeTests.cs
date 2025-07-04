using System;
using System.IO;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;

namespace Wrecept.UiTests;

[TestClass]
public class ScreenModeTests
{
    private static string ExePath =>
        Path.GetFullPath(Path.Combine("..", "..", "..", "..", "Wrecept.Wpf", "bin", "Debug", "net8.0-windows", "Wrecept.Wpf.exe"));
    private const string WinAppDriverUrl = "http://127.0.0.1:4723";

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
                    // no confirmation button present
                }
            }
        }
    }

    [TestMethod]
    public void ScreenSettings_OpensAndCancels()
    {
        using var driver = LaunchApp();

        driver.FindElementByName("Szerviz").Click();
        driver.FindElementByName("Képernyő beállítása").Click();

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        var window = wait.Until(d => d.FindElementByName("Képernyőbeállítás"));
        window.FindElementByName("Mégse").Click();

        Assert.ThrowsException<OpenQA.Selenium.WebDriverException>(() => driver.FindElementByName("Képernyőbeállítás"));
        driver.Close();
    }
}
