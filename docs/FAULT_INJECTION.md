---
title: "Fault Injection"
purpose: "Scenarios for reliability testing and expected mitigation"
author: "docs_agent"
date: "2025-06-27"
---

# 🐞 Fault Injection Scenarios

Ez a dokumentum részletezi, hogyan teszteljük a Wrecept hibákkal szembeni ellenállását.

## Szimulált helyzetek

1. **Adatbázis elvesztése** – A SQLite fájl törlését vagy átnevezését követően a rendszernek biztonságosan alaphelyzetbe kell állnia, és a felhasználót tájékoztatnia kell a helyreállítás lépéseiről.
2. **Sérült konfiguráció** – Hibás vagy hiányzó beállítófájl esetén az alkalmazás alapértelmezett konfigurációval indul, és figyelmeztetést jelenít meg.
3. **Betelt háttértár** – Írási hibát szimulálunk, amikor nincs szabad hely. A tranzakcióknak megszakítás nélkül vissza kell gördülniük, a felhasználó pedig értesítést kap.

## Elvárt viselkedés

* Minden hibaeseményt naplózunk a `%AppData%/Wrecept/logs` könyvtárba.
* A működésnek az adatbiztonság és a felhasználó tájékoztatása mellett kell folytatódnia, vagy szükség esetén biztonságosan leállnia.

---
