using System;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Wrecept.UiTests;

[TestClass]
public class LookupFocusTests
{
    private static WindowsDriver<WindowsElement> LaunchApp() => TestHelper.LaunchApp();

    [TestInitialize]
    public void Setup() => TestHelper.PrepareSettings(false);

    [TestMethod]
    public void SmartLookup_Enter_MovesFocusToNextControl()
    {
        using var driver = LaunchApp();

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        var product = wait.Until(d => d.FindElementByAccessibilityId("EntryProduct"));
        product.Click();

        new Actions(driver).SendKeys(Keys.Enter).Perform();
        var active = driver.SwitchTo().ActiveElement();
        Assert.AreEqual("EntryQuantity", active.GetAttribute("AutomationId"));

        driver.Close();
    }
}
