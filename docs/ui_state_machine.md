---
title: "UI állapotgép"
purpose: "Az AppInteractionState és a fő átmenetek szerepének leírása"
author: "docs_agent"
date: "2025-07-06"
---

# AppInteractionState szerepe

Az alkalmazás felhasználói felülete egy egyszerű állapotgépen keresztül vezérelhető. Az `AppInteractionState` enum minden jelentősebb nézethez vagy művelethez külön állapotot biztosít. Az aktuális állapotot az `AppStateService` tartja nyilván és a `state.json` fájlban menti. A `FocusManager` és a `KeyboardManager` figyeli az `InteractionStateChanged` eseményt, így az éppen aktív nézet automatikusan fókuszt kap és a billentyűk a megfelelő kezelőhöz kerülnek.

## Fő állapotok és átmenetek

- **Startup → MainMenu** – az inicializálás és az előző állapot betöltése után.
- **MainMenu → EditingInvoice** – számlaszerkesztő nézet kiválasztásakor vagy állapot visszatöltésekor.
- **MainMenu → EditingMasterData** – bármely törzsadat menüpont aktiválásakor.
- **MainMenu → BrowsingInvoices** – listák vagy leltár megnyitásakor.
- **EditingInvoice → InlineCreatorActive** – új termék vagy szállító inline létrehozásakor.
- **InlineCreatorActive → EditingInvoice** – a létrehozó ablak bezárásával.
- **EditingInvoice → InlinePromptActive** – sor mentése vagy törlése előtti megerősítő kérdésnél.
- **InlinePromptActive → EditingInvoice** – a prompt bezárásakor.
- **Bármely állapot → DialogOpen** – modális ablak (pl. biztonsági mentés) megnyitásakor, bezárás után visszaáll az előző érték.
- **ExitApplication → Exiting** – a program leállítása előtt.

Ez a központi állapotkezelés garantálja, hogy minden ViewModel csak a saját felelősségébe tartozó állapotot változtassa, a fókusz- és billentyűkezelés pedig egyetlen helyre koncentrálódik.
