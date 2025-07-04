using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace Wrecept.UiTests;

[TestClass]
public class StartupWindowTests
{
    private const string ExePath = @"C:\Users\tankoferenc\source\repos\luckydizzier\wrecept\Wrecept.Wpf\bin\Debug\net8.0-windows\Wrecept.Wpf.exe";
    private const string WinAppDriverUrl = "http://127.0.0.1:4723";

    private static WindowsDriver<WindowsElement> LaunchApp()
    {
        var options = new AppiumOptions();
        options.AddAdditionalCapability("app", ExePath);
        return new WindowsDriver<WindowsElement>(new Uri(WinAppDriverUrl), options);
    }

    [TestMethod]
    public void Application_Launches_And_Closes()
    {
        using var driver = LaunchApp();
        Assert.AreEqual("Wrecept", driver.Title);
        driver.Close();
    }

    [TestMethod]
    public void SeedOptions_Cancel_OpensMainWindow()
    {
        using var driver = LaunchApp();

        var optionsWindow = driver.FindElementByName("Mintaszámok");
        optionsWindow.FindElementByName("Mégse").Click();

        var mainWindow = driver.FindElementByName("Wrecept");
        Assert.IsNotNull(mainWindow);
        driver.Close();
    }

    [TestMethod]
    public void SeedOptions_Ok_ShowsStartupWindow()
    {
        using var driver = LaunchApp();

        var optionsWindow = driver.FindElementByName("Mintaszámok");
        optionsWindow.FindElementByName("OK").Click();

        var startupWindow = driver.FindElementByName("Indulás");
        Assert.IsNotNull(startupWindow);

        driver.Close();
    }
}
