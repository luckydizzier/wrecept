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

    [TestInitialize]
    public void Setup() => TestHelper.PrepareSettings(false);

    [TestMethod]
    public void ScreenSettings_OpensAndCancels()
    {
        using var driver = LaunchApp();

        driver.FindElement(By.Name("Szerviz")).Click();
        driver.FindElement(By.Name("Képernyő beállítása")).Click();

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        var window = wait.Until(d => ((WindowsDriver<WindowsElement>)d)
            .FindElement(By.Name("Képernyőbeállítás")));
        window.FindElement(By.Name("Mégse")).Click();

        Assert.ThrowsException<OpenQA.Selenium.WebDriverException>(() => driver.FindElement(By.Name("Képernyőbeállítás")));
        driver.Close();
    }
}
