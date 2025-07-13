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
   dotnet build InvoiceApp.sln
   ```
4. **Futtatás** fejlesztői módban:
   ```bash
   dotnet run --project Wrecept.Wpf
   ```

## Navigáció a projektben

 - A megoldás három fő projektet tartalmaz: `InvoiceApp.Core`, `InvoiceApp.Data` és `InvoiceApp.MAUI`.
- A nézetmodellek és szolgáltatások elrendezését a [PROJECT_STRUCTURE.md](../PROJECT_STRUCTURE.md) ismerteti.
- A felhasználói felület billentyűs működését a [UI_FLOW.md](../UI_FLOW.md) dokumentum részletezi.
- A billentyűparancsokat a `KeyboardManager` delegálja az egyes
  `*KeyboardHandler` osztályoknak az alkalmazás állapotától függően.
  Az `InvoiceEditorKeyboardHandler` csak akkor reagál az Enter/Esc/Delete
  billentyűkre, ha a fókusz a sorbeviteli mezőknél vagy a terméktáblán van.

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


