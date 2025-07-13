---
title: "Hibabefecskendezési terv"
purpose: "Forgatókönyvek és helyreállítási lépések megbízhatósági teszthez"
author: "root_agent"
date: "2025-06-27"
---

# 🐞 Hibabefecskendezési terv

Ez a dokumentum meghatározza, hogyan vizsgáljuk az alkalmazás hibával szembeni ellenállását.

## Tesztesetek

1. **Adatbázis kapcsolat megszakadása** – Szimuláljuk az elérhetetlenséget és figyeljük, hogy a Storage réteg hogyan kezeli a tranzakciók visszagörgetését.
2. **Hiányzó vagy sérült konfigurációs fájl** – Indításkor ellenőrizzük, hogy a beállítások olvashatók-e; hiba esetén alapértelmezett értékeket töltünk.
3. **Nem várt kivétel a ViewModelben** – Ellenőrizzük, hogy az Error Handling terv szerint logol-e és jelzi-e a hibát a felhasználónak.
4. **Sérült adatbázis szerkezet** – Az `IDbHealthService` `PRAGMA integrity_check` futtatásával vizsgálja a táblák épségét. Hibát a LogService naplóz.
5. **Fájl szintű korrupció** – A `DatabaseRecoveryService` a sérült `app.db` állományt átmásolja a `backup` mappába, majd újraépíti az adatbázist és a `ChangeLog` segítségével visszatölti a rekordokat.
6. **Áramszünet utáni helyreállítás** – A SessionService eltárolja az utoljára szerkesztett számla azonosítóját. A *Szerviz > Áramszünet után* menüpont visszatölti ezt az állapotot.

## Cél

A hibabefecskendezés révén igazolni tudjuk, hogy a rendszer hiba esetén is kontrolláltan működik tovább vagy elegánsan leáll.

---
