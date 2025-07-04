using System;
using System.IO;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Wrecept.UiTests;

[TestClass]
public class StageViewFocusUITests
{
    private const string ExePath = @"C:\Users\tankoferenc\source\repos\luckydizzier\wrecept\Wrecept.Wpf\bin\Debug\net8.0-windows\Wrecept.Wpf.exe";
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
                catch (WebDriverException)
                {
                    // optional window
                }
            }
        }
    }

    [TestMethod]
    public void EscapeRestoresMenuFocus()
    {
        using var driver = LaunchApp();

        driver.FindElementByName("Törzsek").Click();
        driver.FindElementByName("Termékek").Click();

        new Actions(driver).SendKeys(Keys.Escape).Perform();
        var active = driver.SwitchTo().ActiveElement();
        Assert.AreEqual("Termékek", active.Text);

        driver.Close();
    }
}
