using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text.Json;
using Wrecept.Core.Entities;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Wrecept.UiTests;

[TestClass]
public class StartupWindowTests
{
    private static void PrepareSettings(bool firstRun) => TestHelper.PrepareSettings(firstRun);

    private static WindowsDriver<WindowsElement> LaunchApp(bool waitMainWindow = false) =>
        TestHelper.LaunchApp(waitMainWindow);

    [TestMethod]
    public void Application_Launches_And_Closes()
    {
        PrepareSettings(firstRun: false);
        using var driver = LaunchApp(true);
        Assert.AreEqual("Wrecept", driver.Title);
        driver.Close();
    }

    [TestMethod]
    public void SeedOptions_Cancel_OpensMainWindow()
    {
        PrepareSettings(firstRun: false);
        using var driver = LaunchApp();

        var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(5));
        var optionsWindow = wait.Until(d => ((WindowsDriver<WindowsElement>)d)
            .FindElement(By.Name("Mintaszámok")));
        optionsWindow.FindElement(By.Name("Mégse")).Click();

        var mainWindow = wait.Until(d => ((WindowsDriver<WindowsElement>)d)
            .FindElement(By.Name("Wrecept")));
        Assert.IsNotNull(mainWindow);
        driver.Close();
    }

    [TestMethod]
    public void SeedOptions_Ok_ShowsStartupWindow()
    {
        PrepareSettings(firstRun: false);
        using var driver = LaunchApp();

        var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(5));
        var optionsWindow = wait.Until(d => ((WindowsDriver<WindowsElement>)d)
            .FindElement(By.Name("Mintaszámok")));
        optionsWindow.FindElement(By.Name("OK")).Click();

        var startupWindow = wait.Until(d => ((WindowsDriver<WindowsElement>)d)
            .FindElement(By.Name("Indulás")));
        Assert.IsNotNull(startupWindow);

        driver.Close();
    }

    [TestMethod]
    public void UserInfoEditor_FillFields_Confirms()
    {
        PrepareSettings(firstRun: true);
        using var driver = LaunchApp();

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        var setupWindow = wait.Until(d => ((WindowsDriver<WindowsElement>)d)
            .FindElement(By.Name("Első indítás")));
        setupWindow.FindElement(By.Name("OK")).Click();

        var editorWindow = wait.Until(d => ((WindowsDriver<WindowsElement>)d)
            .FindElement(By.Name("Tulajdonosi adatok")));
        wait.Until(d => ((WindowsDriver<WindowsElement>)d)
            .FindElement(By.Id("CompanyNameBox"))).SendKeys("Teszt Kft.");
        driver.FindElement(By.Id("AddressBox")).SendKeys("Siklós, Fő utca 1.");
        driver.FindElement(By.Id("PhoneBox")).SendKeys("1111");
        driver.FindElement(By.Id("EmailBox")).SendKeys("teszt@example.hu");
        driver.FindElement(By.Id("TaxNumberBox")).SendKeys("123");
        driver.FindElement(By.Id("BankAccountBox")).SendKeys("111");
        new Actions(driver).SendKeys(OpenQA.Selenium.Keys.Enter).Perform();

        wait.Until(d => ((WindowsDriver<WindowsElement>)d)
            .FindElement(By.Name("Wrecept")));
        driver.Close();
    }

    [TestMethod]
    public void SetupWindow_Cancel_ClosesApplication()
    {
        PrepareSettings(firstRun: true);
        using var driver = TestHelper.LaunchAppRaw();

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        var setupWindow = wait.Until(d => ((WindowsDriver<WindowsElement>)d)
            .FindElement(By.Name("Első indítás")));
        setupWindow.FindElement(By.Name("Mégse")).Click();

        Assert.ThrowsException<WebDriverException>(() => _ = driver.Title);
    }

    [TestMethod]
    public void UserInfoEditor_Cancel_ClosesApplication()
    {
        PrepareSettings(firstRun: true);
        using var driver = TestHelper.LaunchAppRaw();

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        var setupWindow = wait.Until(d => ((WindowsDriver<WindowsElement>)d)
            .FindElement(By.Name("Első indítás")));
        setupWindow.FindElement(By.Name("OK")).Click();

        var editorWindow = wait.Until(d => ((WindowsDriver<WindowsElement>)d)
            .FindElement(By.Name("Tulajdonosi adatok")));
        editorWindow.FindElement(By.Name("Mégse")).Click();

        Assert.ThrowsException<WebDriverException>(() => _ = driver.Title);
    }
}
