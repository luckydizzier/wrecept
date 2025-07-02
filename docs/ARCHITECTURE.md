---
title: "Architecture Blueprint"
purpose: "Overall module and data flow overview"
author: "docs_agent"
date: "2025-06-27"
---

# 🏗️ Architecture Blueprint

Az alkalmazás rétegei tisztán el vannak választva, hogy a karbantarthatóság és a bővíthetőség hosszú távon biztosított legyen.

## Rétegzett felépítés

1. **UI (Views / Themes)** – XAML nézetek és stílusfájlok a `Wrecept.Wpf` projektben. Csak vizuális elemeket tartalmaz, logikát nem.
   *A globális RetroTheme.xaml minden vezérlőre egységes stílust ad.*
2. **ViewModel** – A CommunityToolkit.Mvvm segítségével kezeli a felhasználói interakciókat és az adatkötéseket.
3. **Core** – Domain modellek, szolgáltatás interfészek és belső számítások.
4. **Storage** – SQLite + Entity Framework Core konténer, migrációk, repositoryk.

Minden réteg csak az alatta lévőt éri el, közvetlen átjárás nem megengedett.
Az elsődleges ablak a `MainWindow`, amely a `StageView` kontrollt jeleníti meg.
`StageView` a menük és az állapotsáv kerete, `StageViewModel` cseréli a tartalmat
 (`InvoiceEditor`, `ProductMaster`, `SupplierMaster`).

## Adatáramlás

```
UI → ViewModel → Core → Storage
```

A felhasználói események a ViewModelen keresztül jutnak el a Core-hoz, amely szükség esetén meghívja a Storage réteg szolgáltatásait.

### Adatáramlás részletesen

1. **View** – A felhasználói műveletek (billentyűleütések) eseményeket generálnak.
2. **ViewModel** – A `RelayCommand` hívásain keresztül validálja az adatot, majd meghívja a Core szolgáltatásait.
3. **Core** – Feldolgozza a kérést, számításokat végez, majd repositorykat hív.
4. **Storage** – Az adatbázisműveletek végrehajtása után visszatér a Core réteghez, amely a ViewModelen keresztül értesíti a View-t.

## EF Core kezelése

Az `DbContext` példányai a Storage rétegben élnek. A migrációk és a sémafrissítések parancssori eszközzel, CI környezetben futnak. A ViewModel soha nem fér közvetlenül az adatbázishoz.

Az adatlekérést repositoryk végzik, amelyek `IInvoiceRepository`, `IProductRepository` és `ISupplierRepository` interfészeket valósítanak meg. Ezek felelősek a hibák naplózásáért és az üres listákkal való visszatérésért hiba esetén.
Ezek fölött `InvoiceService`, `ProductService` és mostantól `SupplierService` gondoskodik a validálásról és a ViewModel réteg kiszolgálásáról.
Az `InvoiceService` kezeli a fejléc frissítését (`UpdateInvoiceHeaderAsync`) és az archiválást.

Minden hibát az `ILogService` rögzít, amelyet a Storage réteg `LogService` implementációja valósít meg. A naplók a `%AppData%/Wrecept/logs` mappában napi bontású fájlokba kerülnek.
Felhasználói üzenetekhez az `INotificationService` ad egységes felületet. WPF alatt a `MessageBoxNotificationService` jeleníti meg a dialógusokat, míg a tesztekben egy csonk "MockNotificationService" működik.
Az alapvető cégadatokat a `UserInfoService` kezeli. Az adatok a `%AppData%/Wrecept/wrecept.json` fájlban tárolódnak, betöltésük az alkalmazás futása közben történik.
Az aktuális képernyőmódot a `SettingsService` tartja nyilván `settings.json` fájlban, amit a `ScreenModeManager` olvas be induláskor.

Minden domain modell tartalmaz `CreatedAt` és `UpdatedAt` mezőket. Ezeket a service réteg inicializálja, így naplózható az adat módosításának ideje.

Az alkalmazás indításakor a `DbInitializer` futtatja a szükséges migrációkat.
Az `AddStorage` kiterjesztés ehhez már `IDbContextFactory`-t vesz igénybe,
így a migráció egy külön kontextuson történik és azonnal eldobásra kerül.
Ezt követően a `DataSeeder` – ha az adatbázis üres vagy hiányzik – saját
kontextust hoz létre és egy minimális mintaadatkészletet tölt be.
Amennyiben csak ez a mintaadatkészlet érhető el, a UI figyelmezteti a felhasználót.
Ha a második adatlekérdezés is hibát jelez, a részleteket az `ILogService` naplózza a `logs` mappába, és a program hibát jelez.

## Indítási folyamat

Az alkalmazás betöltésekor a `StartupOrchestrator` fut le, amely két szintű előrehaladási értéket jelent az UI felé. A `ProgressViewModel` által kötött nézet két `ProgressBar`-on keresztül mutatja a globális és részfeladatok százalékos állását, így a felhasználó valós időben látja a migráció és a mintaadatok betöltésének állapotát.

---
