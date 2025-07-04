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
    private static WindowsDriver<WindowsElement> LaunchApp() => TestHelper.LaunchApp();

    [TestInitialize]
    public void Setup() => TestHelper.PrepareSettings(false);

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
