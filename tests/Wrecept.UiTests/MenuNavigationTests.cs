using System;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace Wrecept.UiTests;

[TestClass]
public class MenuNavigationTests
{
    private static WindowsDriver<WindowsElement> LaunchApp()
    {
        var exePath = Path.GetFullPath(Path.Combine("..", "..", "..", "..", "Wrecept.Wpf", "bin", "Debug", "net8.0-windows", "Wrecept.Wpf.exe"));
        var options = new AppiumOptions();
        options.AddAdditionalCapability("app", exePath);
        var driver = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), options);
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
    public void AboutMenu_DisplaysAboutView()
    {
        using var driver = LaunchApp();

        driver.FindElementByName("Névjegy").Click();
        driver.FindElementByName("A program felhasználójának adatai").Click();

        Assert.IsTrue(driver.PageSource.Contains("Program neve"));
        driver.Close();
    }

    [TestMethod]
    public void ExitMenu_ClosesApplication()
    {
        using var driver = LaunchApp();

        driver.FindElementByName("Vége").Click();
        driver.FindElementByName("Kilépés").Click();
        Thread.Sleep(1000);

        Assert.ThrowsException<WebDriverException>(() => _ = driver.Title);
    }
}
