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

    [TestMethod]
    public void EscapeInUserInfoEditorMovesFocusBack()
    {
        using var driver = LaunchApp();

        driver.FindElementByName("Névjegy").Click();
        driver.FindElementByName("A program felhasználójának adatai").Click();

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        var address = wait.Until(d => ((WindowsDriver<WindowsElement>)d).FindElementByAccessibilityId("AddressBox"));
        address.Click();

        new Actions(driver).SendKeys(Keys.Escape).Perform();
        var active = driver.SwitchTo().ActiveElement();
        Assert.AreEqual("CompanyNameBox", active.GetAttribute("AutomationId"));

        driver.Close();
    }
}
