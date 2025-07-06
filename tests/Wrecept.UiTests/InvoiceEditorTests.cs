using System;
using System.IO;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;

namespace Wrecept.UiTests;

[TestClass]
public class InvoiceEditorTests
{
    private static WindowsDriver<WindowsElement> LaunchApp() => TestHelper.LaunchApp();

    [TestInitialize]
    public void Setup() => TestHelper.PrepareSettings(false);

    [TestMethod]
    public void InvoiceEditor_Loads_DefaultView()
    {
        using var driver = LaunchApp();

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        var list = wait.Until(d => ((WindowsDriver<WindowsElement>)d)
            .FindElementByAccessibilityId("InvoiceList"));
        Assert.IsNotNull(list);
        var entry = driver.FindElementByAccessibilityId("EntryProduct");
        Assert.IsNotNull(entry);

        driver.Close();
    }

    [TestMethod]
    public void InvoiceList_UpOnFirstItem_OpensPrompt()
    {
        using var driver = LaunchApp();

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

        var options = driver.FindElementsByName("Mintaszámok");
        if (options.Count > 0)
            options[0].FindElementByName("Mégse").Click();

        var list = wait.Until(d => ((WindowsDriver<WindowsElement>)d)
            .FindElementByAccessibilityId("InvoiceList"));
        list.Click();

        new OpenQA.Selenium.Interactions.Actions(driver)
            .SendKeys(OpenQA.Selenium.Keys.Insert)
            .Perform();

        var box = wait.Until(d => ((WindowsDriver<WindowsElement>)d)
            .FindElementByAccessibilityId("NumberBox"));
        new OpenQA.Selenium.Interactions.Actions(driver)
            .SendKeys(OpenQA.Selenium.Keys.Enter)
            .Perform();
        wait.Until(d => d.FindElementsByAccessibilityId("NumberBox").Count == 0);

        list.Click();
        new OpenQA.Selenium.Interactions.Actions(driver)
            .SendKeys(OpenQA.Selenium.Keys.ArrowUp)
            .Perform();

        box = wait.Until(d => ((WindowsDriver<WindowsElement>)d)
            .FindElementByAccessibilityId("NumberBox"));
        Assert.IsNotNull(box);

        driver.Close();
    }
}
