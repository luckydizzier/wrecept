---
title: "Felhasználói konfiguráció"
purpose: "Cégadatok tárolása"
author: "docs_agent"
date: "2025-07-01"
---

A program a felhasználó cégadatait JSON formátumban tárolja a `%AppData%/Wrecept/wrecept.json` (vagy a telepítéskor megadott) fájlban. A fájl a következő mezőket tartalmazza:

- `CompanyName` – a vállalkozás neve
- `Address` – székhely vagy telephely címe
- `Phone` – kapcsolattartó telefonszáma
- `Email` – általános elérhetőség
- `TaxNumber` – hivatalos adószám
- `BankAccount` – bankszámlaszám a kiállított számlákhoz

## Billentyűzetprofil

A `Keyboard` szekció határozza meg az alapértelmezett billentyűket:

- `Next` – következő vezérlőre lépés
- `Previous` – előző vezérlőre lépés
- `Confirm` – megerősítő művelet
- `Cancel` – visszalépés vagy bezárás

Az értékek a `System.Windows.Input.Key` enum tagjai, például:

```json
"Keyboard": {
  "Next": "Down",
  "Previous": "Up",
  "Confirm": "Enter",
  "Cancel": "Escape"
}
```

A `UserInfoService` tölti be és menti az adatokat. Hiányzó fájl esetén üres értékeket használ. A `Tulajdonos szerkesztése...` menüpont megnyit egy űrlapot, amely ezen mezőket módosítja.
Az alkalmazás első indításakor lehetőség van a konfigurációs fájl helyének megadására.
