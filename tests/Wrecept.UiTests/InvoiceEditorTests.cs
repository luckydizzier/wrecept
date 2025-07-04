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
}
