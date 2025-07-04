using System;
using System.IO;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Wrecept.UiTests;

[TestClass]
public class MasterDataNavigationTests
{
    private static WindowsDriver<WindowsElement> LaunchApp() => TestHelper.LaunchApp();

    [TestInitialize]
    public void Setup() => TestHelper.PrepareSettings(false);

    [DataTestMethod]
    [DataRow("Termékek")]
    [DataRow("Szállítók")]
    [DataRow("Mértékegységek")]
    public void MasterView_Opens_FromMenu(string item)
    {
        using var driver = LaunchApp();

        driver.FindElementByName("Törzsek").Click();
        driver.FindElementByName(item).Click();

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        var grid = wait.Until(d => ((WindowsDriver<WindowsElement>)d)
            .FindElementByAccessibilityId("Grid"));
        Assert.IsNotNull(grid);

        new Actions(driver).SendKeys(Keys.Escape).Perform();
        driver.Close();
    }
}
