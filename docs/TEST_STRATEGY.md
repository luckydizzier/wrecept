---
title: "Testing Strategy"
purpose: "Unit, integration and UI testing principles"
author: "root_agent"
date: "2025-06-27"
---

# 🧪 Testing Strategy

A Wrecept stabilitását több szinten biztosítjuk.

## Tesztszintek

1. **Unit tesztek** – A Core és ViewModel rétegek logikáját izoláltan ellenőrizzük.
2. **Integration tesztek** – Adatbázis műveletek és szolgáltatások együttműködését vizsgáljuk SQLite in-memory módban.
3. **UI tesztek** – A WPF nézetek billentyű-kezelését automatizáltan teszteljük, például WinAppDriverrel.

## Hülyebiztos validáció

* Null-check minden publikus belépési ponton.
* Alapértelmezett értékek biztosítása, ahol a modell megengedi.
* Adatintegritás ellenőrzése a Storage rétegben tranzakciókkal.

## Coverage és CI

* Minimum 80% kódfedettségre törekszünk, de a kritikus útvonalakat minden esetben lefedjük.
* A tesztek a CI folyamat részeként futnak, hibás build nem kerülhet kiadásra.

---
