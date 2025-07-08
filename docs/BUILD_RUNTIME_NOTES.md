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
- **Téves karakter a `DataTemplate`-ben** – ha egy véletlen karakter (pl. `z`) marad a sablon nyitó vagy záró tagja előtt, MC3061 hibával leáll a fordítás.

## Javaslatok

1. Minden nézetnél használjunk egyedi, a típus nevétől eltérő `x:Name` értékeket (pl. `InvoiceEditorHost`), így elkerülhető a névütközés.
2. Fordítás előtt futtassunk `dotnet build`-et és figyeljük a figyelmeztetéseket.
3. A XAML fájlokat mindig ellenőrizzük jól formázott XML szempontjából.
4. Teszteléskor győződjünk meg róla, hogy a szükséges SDK-k és NuGet csomagok telepítve vannak.
5. Sémafrissítés után futtassuk le az EF Core migrációkat (`Database.Migrate()`),
   különben futásidőben "no such column" hibát kaphatunk.
6. Indításkor a `DatabaseInitializer` végzi a sémamigrációt és a `PRAGMA journal_mode=WAL` parancs futtatását. A folyamat `IDbContextFactory` segítségével külön kontextusban zajlik.
    Ha az adatbázis üres, a felhasználó megerősítheti, hogy Bogus segítségével generált mintaadatok kerüljenek be.
7. Az `AddStorageAsync` kiterjesztés már csak a szolgáltatásokat regisztrálja, migrációt nem indít.
8. Ha a második adatlekérdezés is `SqliteException`-t dob, a `DataSeeder` a `logs/startup.log` fájlba ír és `Failed` állapotot jelez.
9. Új modell bevezetésekor, ha valamely tábla hiányzik, a `DataSeeder` ismét migrációt futtat és naplózza a hibát.
10. A `SetupWindow` bezárása után az alkalmazás alapértelmezett `OnLastWindowClose` módja miatt azonnal leállt,
    ezért a `ShutdownMode` beállítása a `OnStartup` végén `InvalidOperationException`-t dobott.
    A megoldás: `OnStartup` elején állítsuk `ShutdownMode = OnExplicitShutdown` értékre.
11. Amennyiben sem adatbázis, sem konfigurációs fájl nem létezik, indításkor rákérdezünk:
    "Biztos, hogy elölről kezded?". Elfogadás után a `SetupWindow` és a
    tulajdonosi adatok szerkesztője jelenik meg, ezek mentik a beállított
    elérési utakat és cégadatokat.
12. A `StartupWindow` `Mégse` gombjára kattintva `ObjectDisposedException` lépett
    fel, ha a `CancellationTokenSource` már felszabadult. A parancs most
    biztonságosan kezeli ezt és az indítás végén eltávolítjuk a hivatkozást.

---
