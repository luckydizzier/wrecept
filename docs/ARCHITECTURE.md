---
title: "Architecture Blueprint"
purpose: "Overall module and data flow overview"
author: "docs_agent"
date: "2025-06-27"
---

# 🏗️ Architecture Blueprint

Az alkalmazás rétegei tisztán el vannak választva, hogy a karbantarthatóság és a bővíthetőség hosszú távon biztosított legyen.

## Rétegzett felépítés

1. **UI (Views / Themes)** – XAML nézetek és stílusfájlok az `InvoiceApp.MAUI` projektben. Csak vizuális elemeket tartalmaznak, platformfüggő logika nélkül.
   *A globális RetroTheme.xaml minden vezérlőre egységes stílust ad.*
2. **ViewModel** – A CommunityToolkit.Mvvm segítségével kezeli a felhasználói interakciókat és az adatkötéseket.
3. **Core** – Domain modellek, szolgáltatás interfészek és belső számítások az `InvoiceApp.Core` projektben.
4. **Storage** – SQLite + Entity Framework Core konténer, migrációk és repositoryk az `InvoiceApp.Data` projektben.

Minden réteg csak az alatta lévőt éri el, közvetlen átjárás nem megengedett.
Az elsődleges ablak a `MainPage`, amely a `StageView` kontrollt jeleníti meg.
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
Fejléc mentésekor az `InvoiceLookupViewModel` újratölti a listát és a módosított számlát jelöli ki.
Az új `RemoveItemAsync` metódus lehetővé teszi egy meglévő tétel törlését adatbázisból.
Az `IInvoiceExportService` felülete biztosít PDF mentést és nyomtatást, a `PdfInvoiceExporter` a MAUI felületen keresztül jeleníti meg a dokumentumokat.

Az `OnModelCreating` metódus indexeket készít a gyakran szűrt mezőkre:
`Invoices.Date`, `Invoices.SupplierId` és `Products.Name`. A korábbi
`TaxRate` és `Unit` indexekhez hasonlóan ezek is a lekérdezések gyorsítását
segítik.

Minden hibát az `ILogService` rögzít, amelyet a Storage réteg `LogService` implementációja valósít meg. A naplók a `%AppData%/InvoiceApp/logs` mappában napi bontású fájlokba kerülnek.
Felhasználói üzenetekhez az `INotificationService` ad egységes felületet. MAUI alatt a `MessageBoxNotificationService` jeleníti meg a dialógusokat, míg a tesztekben egy csonk "MockNotificationService" működik.
Az alapvető cégadatokat a `UserInfoService` kezeli. Az adatok a `%AppData%/InvoiceApp/invoiceapp.json` fájlban tárolódnak, betöltésük az alkalmazás futása közben történik.
Az aktuális képernyőmódot a `SettingsService` tartja nyilván `settings.json` fájlban, amit a `ScreenModeManager` olvas be induláskor.
Az adatbázis integritását az `IDbHealthService` ellenőrzi `PRAGMA integrity_check` parancs futtatásával.
Az utolsó megnyitott számla azonosítóját a `SessionService` jegyzi meg a `session.json` fájlban.
Az `AppStateService` az aktuális nézetet és az `AppInteractionState` értékét tárolja, amit a nézetek és a billentyűkezelés figyel a kiszámítható átmenetek érdekében.
Ezt a változást a `FocusManager` automatikus fókuszváltással, a `KeyboardManager` pedig állapottól függő billentyűkezeléssel követi.
Az `StageViewModel`, `InvoiceEditorViewModel` és `InvoiceLookupViewModel` `AddSingleton` élettartammal kerülnek a DI-konténerbe, így a billentyűkezelők és a nézetek ugyanazon példányokkal dolgoznak.
Az állapot szolgáltatás a `state.json` fájlba menti az utolsó aktív menüpontot és a szerkesztett számla azonosítóját, amit indításkor visszatöltünk.

### Törzsadatok szerkesztése

A különböző mesteradat nézeteket kiszolgáló ViewModel-ek mind az `EditableMasterDataViewModel<T>` osztályból öröklődnek. Ez az alaposztály valósítja meg az egységes `EditSelectedCommand`, `DeleteSelectedCommand` és `CloseDetailsCommand` parancsokat, továbbá az `IsEditing` flag-et. A törlés valójában archiválás: a leszármazott ViewModel a `DeleteAsync` metódusban hívja meg a hozzá tartozó Service `UpdateAsync` műveletét, miután az entitást archiváltnak jelölte.

Minden domain modell tartalmaz `CreatedAt` és `UpdatedAt` mezőket. Ezeket a service réteg inicializálja, így naplózható az adat módosításának ideje.

 Az alkalmazás indításakor a `DatabaseInitializer` szolgáltatás fut le. Ez a
 `DbInitializer` statikus osztályt hívja a sémamigrációk végrehajtásához, majd
 `PRAGMA journal_mode=WAL` utasítással aktiválja a naplózási módot. A
 migrációkhoz `IDbContextFactory` használatos, így a munkakontextus azonnal
 eldobásra kerül.
Az indítás során a `DataSeeder` ellenőrzi, hogy az adatbázis teljesen üres‑e.
Ha igen, a felhasználó egy párbeszédablakban megadhatja,
hány szállító, termék, számla és tétel generálódjon.
A Bogus könyvtár en_GB lokalizációval hozza létre a mintaszámlákat.
A `StartupWindow` a két ProgressBar segítségével jelzi,
hogy például *Szállítók 3/20* állásnál tart a folyamat.
A mintaadatok betöltése háttérszálon fut, így az UI végig reszponzív marad.
Ha a második adatlekérdezés is hibát jelez, a részleteket az `ILogService` naplózza a `logs` mappába, és a program hibát jelez.

## Indítási folyamat

Az alkalmazás betöltésekor a `StartupOrchestrator` fut le, amely két szintű előrehaladási értéket jelent az UI felé. A `ProgressViewModel` által kötött nézet két `ProgressBar`-on keresztül mutatja a globális és részfeladatok százalékos állását, így a felhasználó valós időben látja a migráció és a mintaadatok betöltésének állapotát.

Az első indításkor a `LoadSettingsAsync` metódus a `ISetupFlow` szolgáltatás segítségével kéri be az adatbázis- és cégadatok elérési útvonalát. A `SetupFlow` alapértelmezett implementáció MAUI oldalakat használ, de tesztben könnyen helyettesíthető.
A folyamat megszakításakor az `IEnvironmentService` hívódik, így a kilépés tesztkörnyezetben is ellenőrizhető.

Az `InvoiceEditorLayout` megnyitásakor hasonló ablak jelenik meg. A törzsadatok (fizetési módok, szállítók, ÁFA‑kulcsok, termékek, mértékegységek) betöltése lépésenként történik, a második sáv pedig az adott lista elemeinek betöltési arányát írja ki.
Az összesítő mezők kiszámítását a `TotalsViewModel`, míg a sorok kezelését az `InvoiceItemEditorViewModel` végzi.

## Dialóguskezelés

A modális ablakok megjelenítését a `NavigationService.ShowCenteredDialog` koordinálja. A metódus a `MainPage` példányát állítja be tulajdonosnak, majd a `DialogHelper.CenterToOwner` hívással középre igazítja a párbeszédablakot, mielőtt meghívja a `ShowDialog` függvényt. Így minden dialógus egységesen, ismétlődő pozicionálási kód nélkül jelenik meg.

A progress logok szerint a `DialogService` jelenleg csak az `EditEntity` dialógust indítja el, de később ez a szolgáltatás fogja összefogni a különféle modális ablakok megnyitását és esetleges útvonalkezelését. A cél, hogy a ViewModel rétegek ne közvetlenül hozzák létre a dialógusokat, hanem a `DialogService` döntse el, miként és mely nézetekkel jelenjenek meg.

## MAUI átállás előkészítése

A tervek szerint a projektet többplatformos .NET MAUI megoldássá alakítjuk. Az új felépítés a következő rétegekre bontható:

1. **Projekt Struktúra** – A Visual Studio megoldás három projektből áll majd:
   - **InvoiceApp.MAUI** – a Views, ViewModels, a `StartupOrchestrator` és a lokalizációs erőforrások otthona.
   - **InvoiceApp.Core** – .NET Standard könyvtár a modelleknek és a szolgáltatás interfészeknek.
   - **InvoiceApp.Data** – .NET Standard könyvtár az EF Core `DbContext`-tel és a repositorykkal.

2. **MRS (Model–Repository–Service) Réteg** – az adatkezelés és üzleti logika szervezése:
   - *Models* (`InvoiceApp.Core`): `Invoice`, `InvoiceItem`, `Supplier`, `Product` stb.
   - *Repositories* (`InvoiceApp.Data`): minden modellhez CRUD műveleteket biztosító osztály.
   - *Services* (`InvoiceApp.Core` interfészek, `InvoiceApp.MAUI` implementációk): a ViewModel réteg által használt üzleti logika.

3. **MVVM (Model–View–ViewModel) Réteg** – a felhasználói felület és a logika szétválasztására:
  - *Views* (`InvoiceApp.MAUI`): `MainPage.xaml`, `InvoiceEditor.xaml`, `LookupDialog.xaml`.
   - *ViewModels* (`InvoiceApp.MAUI`): `MainViewModel`, `InvoiceEditorViewModel`, `LookupDialogViewModel` stb.


---
