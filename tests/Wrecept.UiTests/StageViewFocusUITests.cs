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
    private static WindowsDriver<WindowsElement> LaunchApp() => TestHelper.LaunchApp();

    [TestInitialize]
    public void Setup() => TestHelper.PrepareSettings(false);

    [TestMethod]
    public void EscapeRestoresMenuFocus()
    {
        using var driver = LaunchApp();

        driver.FindElementByName("Törzsek").Click();
        driver.FindElementByName("Termékek").Click();

        new Actions(driver).SendKeys(Keys.Escape).Perform();
        var active = driver.SwitchTo().ActiveElement();
        Assert.AreEqual("Termékek", active.GetAttribute("Name"));

        driver.Close();
    }
}
