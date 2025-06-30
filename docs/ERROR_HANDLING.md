---
title: "Error Handling"
purpose: "Runtime and compile-time defense rules"
author: "docs_agent"
date: "2025-06-27"
---

# 🚨 Error Management Plan

Ez a dokumentum összefoglalja a hibakezelési stratégiát. Cél, hogy az alkalmazás összeomlás nélkül, jól nyomon követhetően működjön.

## Kivételkezelés

* A magas szintű beavatkozási pontokon (ViewModel, Service) használunk `try`-`catch` blokkot.
* Csak a szükséges adatot logoljuk, hogy érzékeny információ ne kerüljön ki.
* Kivétel elnyelése helyett visszajelzést adunk a felhasználónak a `feedback_agent` segítségével.

## Aktív védelem

* **Runtime**: Minden bemenetet validálunk a modellbe kerülés előtt. Default értékeket használunk null helyett.
* **Compile-time**: `Nullable` engedélyezett, figyelmeztetések tiltása nem megengedett.

## Naplózás

* A `ILogService` a `%AppData%/Wrecept/logs` könyvtárba ír naplóbejegyzéseket.
* A Storage réteg hibáit a `LogService` rögzíti napi bontású fájlokba.
* Kritikus hiba esetén a felhasználó dönthet a folytatásról vagy kilépésről.

## Konkrét példák

1. **Adatbázis fájl hiánya** – Ha a `wrecept.db` nem található indításkor, a Storage réteg új üres adatbázist hoz létre, majd figyelmeztető üzenetet jelenítünk meg.
2. **Üres adatbázis** – Ha egyetlen táblában sincs adat, minta rekordokat szúrunk be és figyelmeztetjük a felhasználót.
3. **Sémahibák indításkor** – A `DbInitializer` gondoskodik a migrációk lefuttatásáról. Sikertelenség esetén `EnsureCreated()` hívással létrehozza az alap sémát, majd ismét migrál. A `DataSeeder` csak ezután próbál adatot lekérni vagy beszúrni.
4. **Sérült import fájl** – Hibás formátumú vagy hiányzó adatfájl betöltésekor megszakítjuk a folyamatot, naplózzuk a fájl nevét és a kiváltó hibát, és lehetőséget adunk új fájl kiválasztására.
5. **Hálózati kimaradás** – Külső frissítések letöltése közben kapcsolatvesztés esetén újrapróbálkozunk, majd offline módra váltunk, miközben a felhasználót tájékoztatjuk.
6. **Sikertelen adatbázis írás** – Ha a fájl zárolt vagy elfogy a tárhely, hibaüzenetet jelenítünk meg, a műveletet naplózzuk, majd biztonsági mentés után újrapróbáljuk.
7. **Indítási hiba** – Ha a `DataSeeder` másodszori próbálkozásra is `SqliteException`-t kap, a részleteket az `ILogService` naplózza a `logs` mappába, majd hibaüzenetet jelenítünk meg.

---
