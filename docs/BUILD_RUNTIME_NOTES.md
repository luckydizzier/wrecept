---
title: "Build & Runtime Hibák"
purpose: "Tanulságok a korábbi hibákból"
author: "docs_agent"
date: "2025-06-29"
---

# Összegyűjtött hibák és megelőzésük

Ez a jegyzet a fejlesztés során tapasztalt fordítási és futásidejű problémák rövid kivonata. A felsorolt hibák elkerülése érdekében minden módosítás előtt érdemes ellenőrizni a XAML és C# fájlokat.

## Tipikus hibák

- **Hiányzó .NET Desktop Runtime** – a WPF futtatásához szükséges csomag telepítése elmaradt.
- **Helytelen XAML elemnevek** – például `ViewBox` helyett `Viewbox` szerepelt, ami fordítási hibához vezetett.
- **Nem támogatott `{x:Int32}` jelölés** – a Tag attribútumnál az egyszerű numerikus érték használata biztosítja a kompatibilitást.
- **Hiányzó using direktíva** – a `StageView.xaml.cs` állományban a `System.Windows` névtér hiánya CS0246 hibát okozott.
- **Elgépelés vagy hiányzó záró tag** – az `InvoiceEditorView.xaml` fájlban egy fölösleges `</StackPanel>` tag miatt a build meghiúsult.
- **Önzáró `DataGridTemplateColumn` hiánya** – ha a nyitó tag `>` jellel zárul és az attribútumok külön sorban szerepelnek, a parser MC3061 hibát dob.
- **Teszt futtatásához szükséges csomag** – a `Xunit.StaFact` hiánya tesztfutási hibát eredményezhet Windows környezetben.

## Javaslatok

1. Minden nézetnél használjunk egyedi, a típus nevétől eltérő `x:Name` értékeket (pl. `InvoiceEditorHost`), így elkerülhető a névütközés.
2. Fordítás előtt futtassunk `dotnet build`-et és figyeljük a figyelmeztetéseket.
3. A XAML fájlokat mindig ellenőrizzük jól formázott XML szempontjából.
4. Teszteléskor győződjünk meg róla, hogy a szükséges SDK-k és NuGet csomagok telepítve vannak.
5. Sémafrissítés után futtassuk le az EF Core migrációkat (`Database.Migrate()`),
   különben futásidőben "no such column" hibát kaphatunk.
6. Indításkor a `DbInitializer` megpróbálja a `Database.Migrate()` hívást. Ha ez sikertelen, `EnsureCreated()` után ismét migrál. Ha az adatbázis üres, a felhasználó megerősítheti, hogy Bogus segítségével generált mintaadatok kerüljenek be.
7. Az `AddStorage` kiterjesztés migrációhoz `IDbContextFactory`-t használ, így a munkakontextus az inicializálás végén eldobásra kerül.
8. Ha a második adatlekérdezés is `SqliteException`-t dob, a `DataSeeder` a `logs/startup.log` fájlba ír és `Failed` állapotot jelez.
9. Új modell bevezetésekor, ha valamely tábla hiányzik, a `DataSeeder` ismét migrációt futtat és naplózza a hibát.

---
