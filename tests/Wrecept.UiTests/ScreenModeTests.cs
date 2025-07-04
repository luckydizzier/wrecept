using System;
using System.IO;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;

namespace Wrecept.UiTests;

[TestClass]
public class ScreenModeTests
{
    private static WindowsDriver<WindowsElement> LaunchApp() => TestHelper.LaunchApp();

    [TestMethod]
    public void ScreenSettings_OpensAndCancels()
    {
        using var driver = LaunchApp();

        driver.FindElementByName("Szerviz").Click();
        driver.FindElementByName("Képernyő beállítása").Click();

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        var window = wait.Until(d => ((WindowsDriver<WindowsElement>)d)
            .FindElementByName("Képernyőbeállítás"));
        window.FindElementByName("Mégse").Click();

        Assert.ThrowsException<OpenQA.Selenium.WebDriverException>(() => driver.FindElementByName("Képernyőbeállítás"));
        driver.Close();
    }
}
