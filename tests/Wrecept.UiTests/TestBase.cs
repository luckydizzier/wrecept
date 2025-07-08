using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using System;

namespace Wrecept.UiTests
{
    public class TestBase
    {
        protected WindowsDriver<WindowsElement>? _driver;

        [TestInitialize]
        public virtual void TestInitialize()
        {
            TestHelper.PrepareSettings(false);
        }

        [TestCleanup]
        public virtual void TestCleanup()
        {
            if (_driver != null)
            {
                try 
                {
                    _driver.Close();
                }
                catch
                {
                    // Ignore errors during cleanup
                }
                _driver.Dispose();
                _driver = null;
            }
        }

        protected WindowsDriver<WindowsElement> LaunchApp(bool waitMainWindow = true)
        {
            _driver = TestHelper.LaunchApp(waitMainWindow);
            return _driver;
        }
    }
}