---
title: "Fejlesztői útmutató"
purpose: "Útmutató fejlesztők számára"
author: "root_agent"
date: "2025-06-27"
---

# 📙 Fejlesztői útmutató

Helyőrző a magyar fejlesztői útmutatóhoz.


## UI tesztek WinAppDriverrel

A billentyűzettel vezérelt WPF felület automatikus ellenőrzéséhez a [WinAppDriver](https://github.com/microsoft/WinAppDriver) és az Appium kliens használható.

1. Telepítsd a WinAppDriver MSI csomagot, majd indítsd el a `WinAppDriver.exe` alkalmazást.
2. A `tests` könyvtárban hozz létre egy MSTest projektet, majd add hozzá a következő csomagokat:
   ```bash
   dotnet add package MSTest.TestAdapter
   dotnet add package MSTest.TestFramework
   dotnet add package Moq
   ```
3. A tesztből Appiumon keresztül csatlakozz a futó alkalmazáshoz például az alábbi módon:
   ```csharp
   [TestMethod]
   public void InvoiceList_OpenAndClose()
   {
       var options = new AppiumOptions();
       options.AddAdditionalCapability("app", "<path-to>\\Wrecept.Wpf.exe");
       using var driver = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), options);
       // TODO: UI lépések és ellenőrzések
       driver.Close();
   }
   ```
A WinAppDriver headed módban fut, így a tesztekhez lokális Windows GUI szükséges.

