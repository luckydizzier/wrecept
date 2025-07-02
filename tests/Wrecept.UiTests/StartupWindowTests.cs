using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace Wrecept.UiTests;

[TestClass]
public class StartupWindowTests
{
    [TestMethod]
    public void Application_Launches_And_Closes()
    {
        var exePath = Path.GetFullPath(Path.Combine("..", "..", "..", "..", "Wrecept.Wpf", "bin", "Debug", "net8.0-windows", "Wrecept.Wpf.exe"));
        var options = new AppiumOptions();
        options.AddAdditionalCapability("app", exePath);

        using var driver = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), options);

        Assert.AreEqual("Wrecept", driver.Title);
        driver.Close();
    }
}
