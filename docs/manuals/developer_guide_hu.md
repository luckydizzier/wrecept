---
title: "Fejlesztői útmutató"
purpose: "Útmutató fejlesztők számára"
author: "root_agent"
date: "2025-06-27"
---

# 📙 Fejlesztői útmutató

Ez a dokumentum a projekt fejlesztéséhez szükséges alapvető lépéseket tartalmazza. A részletes architektúrai és kódolási irányelvek a [README.md](../README.md) és [CODE_STANDARDS.md](../CODE_STANDARDS.md) fájlokban találhatók.

## Alapvető környezet beállítása

1. **.NET 8 SDK** és a *Windows Desktop Runtime* telepítése.
   ```bash
   dotnet --list-sdks
   ```
   A kimenetben szerepeljen a `8.0.x` verzió.
2. **Repository klónozása** majd megnyitása Visual Studio vagy VS Code alatt.
   ```bash
   git clone <repository-url>
   cd wrecept
   ```
3. **Megoldás fordítása** a gyökérben:
   ```bash
   dotnet build Wrecept.sln
   ```
4. **Futtatás** fejlesztői módban:
   ```bash
   dotnet run --project Wrecept.Wpf
   ```

## Navigáció a projektben

- A megoldás három fő projektet tartalmaz: `Wrecept.Core`, `Wrecept.Storage` és `Wrecept.Wpf`.
- A nézetmodellek és szolgáltatások elrendezését a [PROJECT_STRUCTURE.md](../PROJECT_STRUCTURE.md) ismerteti.
- A felhasználói felület billentyűs működését a [UI_FLOW.md](../UI_FLOW.md) dokumentum részletezi.

## Karbantartási teendők

1. **Tesztelés**: minden módosítás előtt futtasd a teszteket.
   ```bash
   dotnet test
   ```
2. **Adatbázis-migrációk** módosításakor használd az `ef` eszközt.
   ```bash
   dotnet ef migrations add <Név>
   dotnet ef database update
   ```
3. **Haladás naplózása**: minden változtatás után készíts bejegyzést a `docs/progress` mappában a dátum és az agent nevének feltüntetésével.
4. **Új szolgáltatás**: az `InvoiceService` `RemoveItemAsync` metódusa törli a kijelölt tételt. A ViewModel ezt hívja meg a sorok törlésekor.



## UI tesztek WinAppDriverrel

A WPF felület automatikus ellenőrzéséhez a [WinAppDriver](https://github.com/microsoft/WinAppDriver) és az Appium kliens használható.

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
       var list = driver.FindElementByAccessibilityId("InvoiceList");
       Assert.IsNotNull(list);
       driver.Close();
   }
   ```
4. A repóban már szerepel egy minta projekt `tests/Wrecept.UiTests` néven.
   Fordítsd le a `Wrecept.Wpf` alkalmazást `Debug` módban, majd indítsd a WinAppDriver-t és futtasd a teszteket:
   ```bash
   dotnet test tests/Wrecept.UiTests/Wrecept.UiTests.csproj
   ```
A WinAppDriver headed módban fut, így a tesztekhez lokális Windows GUI szükséges.

## Indítási UI tesztek

A `StartupWindowTests` a teljes indulási folyamatot automatizálja. A tesztek a
`tests/Wrecept.UiTests` projektben találhatók. Az alkalmazás elérési útját
a teszt futáskor relatívan számítjuk ki,
így nem függ a fejlesztői könyvtárstruktúrától.

### Tesztek sorrendje

1. **Application_Launches_And_Closes** – egyszerűen megnyitja majd bezárja a főablakot.
2. **SeedOptions_Cancel_OpensMainWindow** – a „Mintaszámok” ablakban a *Mégse* gombra kattint, majd ellenőrzi, hogy a *Wrecept* főablak jelenik meg.
3. **SeedOptions_Ok_ShowsStartupWindow** – az *OK* gombbal elindítja a mintadatok feltöltését, és ellenőrzi, hogy megjelenik az *Indulás* ablak.
4. **UserInfoEditor_FillFields_Confirms** – első indításkor kitölti a tulajdonosi adatokat, majd az *Enter* billentyűvel megerősít.

A tesztek a `settings.json` állomány törlésével vagy létrehozásával különböztetik meg az első indítást a szokásos futástól.

A fenti esetek egyenként futtathatók például:

```bash
dotnet test tests/Wrecept.UiTests/Wrecept.UiTests.csproj --filter "Name=SeedOptions_Ok_ShowsStartupWindow"
```

Az új tesztsegéd automatikusan kezeli az első indítási ablakokat, és hiba esetén képernyőképet ment az aktuális könyvtárba `error_YYYYMMDD_HHMMSS.png` néven.


