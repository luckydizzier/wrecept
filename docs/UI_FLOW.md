UI_FLOW.md

🧱 Overview

This document describes the user interface flow of the Wrecept application. It outlines the navigation model, expected behaviors, data entry sequences, and the logic of interaction across screens and embedded components. It adheres to the current implementation goals defined in BUSINESS_LOGIC.md and supports both keyboard-based workflows and inline editing models.

📌 Navigation Model

Start View: Blank screen with top menu bar focused on Számlák.

Menu Navigation:

Arrow Left/Right: Navigate between main menu tabs: Számlák, Törzsek, Listák, Karbantartás, Névjegy, Kilépés

Arrow Up/Down: Navigate within submenu items

Enter: Activates the selected submenu view and focuses the first control

Escape: Returns to menu with last selected item focused

AccessKey jelek (aláhúzás) segítik az Alt+betű kombinációkat a főmenüben és a
számlafejléc mezőknél.

- Fókuszkezdő pontok nézetenként:
  - **StageView** – a főmenüsor első eleme
  - **InvoiceLookupView** – `InvoiceList` `ListBox`
  - **InvoiceEditorView** – bal oldali `InvoiceList`
  - **ProductMasterView** – a táblázat (Grid)
  - **SupplierMasterView** – a táblázat (Grid)
  
Az adott nézetek betöltésekor a `FocusManager.RequestFocus` hívása állítja be
a kezdő fókuszt a felsorolt vezérlőkre.
A `FocusManager` nyilvántartja az utoljára fókuszált elemeket is, így a nézetek közti váltáskor vagy a dialógusok bezárásakor ugyanide tér vissza a fókusz.

🧾 Invoice Editor Flow (Bejövő szállítólevelek)

1. Invoice Number Field (Lookup & Creation)

A ComboBox-like control at the top shows existing invoice numbers in descending date order

If user attempts to go above topmost item (0th row), an inline new invoice creation is triggered

Confirmation prompt: Create invoice XXXXX1231? (Enter=Yes, Esc=No)

2. Invoice Header Data

After invoice number confirmed:

Supplier selection (SmartLookup)

Date selection (default = today, arrow or numpad)

Payment method (SmartLookup)

Bruttó checkbox (affects unit price interpretation)

3. Invoice Line Items Entry

Focus shifts to the first line’s Product Name

SmartLookup behavior with real-time filtering and keyboard navigation

If product not found → inline product creator in-row (no modal popup)

Pre-fill Quantity, Price, TaxRate based on latest usage

Confirm entry prompt: Save line? (Enter=Yes, Esc=No)

Insert new line, repeat

Quantity < 0 indicates return (visszáru), highlighted red via converter

📄 Invoice Finalization

PDF Export / Print button is only active when IsArchived == true

Archived invoices:

Cannot be edited

Cannot add/remove lines

Display read-only controls

Archive button visible while invoice not archived. Confirmation text: "Véglegesíti a számlát, később nem módosítható."

📊 SmartLookup UX-behavior

All master-data fields (e.g., Supplier, Product, TaxRate, Unit) use a unified SmartLookup component:

Typing filters the list in real time.

Up/Down arrows cycle through the filtered list.

Enter accepts the selected entry and jumps to the next control.

If no match is found and Enter is pressed, inline creation UI appears (InlineCreatorViewModel is set).

Escape cancels editing or closes the inline creator.

Example:

→ User starts typing "tri..."
→ Matches: "Trappista", "Trikolor paprika", etc.
→ ↓ selects "Trappista"
→ Enter → field set to ProductId = 23, focus → Quantity

The SmartLookup behavior ensures consistent UX and keyboard flow across invoice fields.

📀 Screen Mockups

🔳 Main Menu Flow

┌────────────────────────────────────────────────────────────────────────────┐
│ [Számlák] [Törzsek] [Listák] [Karbantartás] [Névjegy] │
│                                                      │
│ > Bejövő szállítólevelek                             │
│   Termékek                                           │
│   Szállítók                                          │
│   ...                                                │
└───────────────────────────────────────────────────────────────────────┘

🧾 Invoice Editor View

┌───── Lista ────┬──────── Számla szerkesztő ────────────────┐
│ [Számlaszám]   │ Szállító: [SmartLookup   ]               │
│ [Dátum]        │ Dátum:    [2025-06-30  ]                │
│ [Szállító]     │ Szám:     [XXXXX1231   ]                │
│                │ Fiz. mód: [SmartLookup   ]   [ ] Bruttó │
│                ├──────────────────────────────────────────┤
│                │ Termék  Menny. Csop. Me.e. Ár  ÁFA       │
│                │ [Edit] [  1] ...                         │
│                │ ... korábban felvitt sorok ...           │
│                │ Enter az első sorban ment és töröl      │
│                │ Esc bármikor visszaugrik a bal oldali    │
│                │ listára                                  │
└────────────────┴──────────────────────────────────────────┘

🔁 Special Behavior

All views must support full keyboard navigation
Key mappings are loaded from `wrecept.json` via `KeyboardProfile`, allowing users to redefine the Next, Previous, Confirm and Cancel keys.

Inline creators must not shift focus away from the current context

Views are loaded in-place inside StageView, avoiding modal disruptions

Menu state persists across Escape presses to return user to most recent focus
The FocusManager.RequestFocus helper accepts an optional view-type
parameter to narrow the search scope; dynamic elements therefore need not use
globally unique identifiers.
FocusManager jegyzi meg, melyik vezérlő volt aktív a nézetekben, így a promptok bezárásakor visszaállítható a fókusz.

📚 Future List Views

Menus will later populate listings (e.g., invoice history, product usage) from their respective modules

No need to implement grid-based listing yet; future enhancement

📌 Constraints

Archive logic must follow business rules (immutable once archived)

Bruttó flag controls pricing behavior throughout lifecycle

UX must reflect availability of actions based on current invoice state

ℹ️ This file is part of the coordinated documentation set along with BUSINESS_LOGIC.md and RefactorPlan.md. Use this UI Flow spec to align visual layout, keyboard handling, and interaction design with core logic and model behavior.

📺 Képernyőmód beállító ablak

A "Karbantartás / Képernyő" menüpont egy kis modális ablakot nyit meg. A `ScreenModeWindow` egy egyszerű `ListBox`-ot tartalmaz, amely az elérhető módokat sorolja fel. Az "OK" gomb az aktuális kiválasztást menti és lezárja az ablakot, a "Mégse" visszalépésre szolgál.

A `ScreenModeViewModel` tölti be az értékeket az `Enum.GetValues<ScreenMode>()` hívással. A `SelectedMode` tulajdonság az `ObservableProperty` attribútumot használja, a `RelayCommand` pedig meghívja a `ScreenModeManager.ChangeModeAsync` metódust. A ViewModel az ablak `DataContext`-jeként működik.

📐 `ScreenModeManager` szerepe

Induláskor a `ScreenModeManager.ApplySavedAsync` kiolvassa a `%AppData%/Wrecept/settings.json` fájlt a `SettingsService` segítségével. A beállított ablakméret és betűméret így visszaáll az előző állapotra. Az új mód kiválasztásakor a szolgáltatás frissíti a főablak méreteit, majd elmenti az értéket a `settings.json`-ba az `ISettingsService.SaveAsync` hívással.

📋 Dialóguskezelés lépései

1. A ViewModel létrehozza a megfelelő szerkesztő ViewModelt (pl. `ProductEditorViewModel`).
2. Az `OnOk` delegáltban frissíti a kiválasztott entitást, majd meghívja a szolgáltatást a mentésre.
3. A `DialogService.EditEntity<TView, TViewModel>` hívás elkészíti az `EditEntityDialog` példányt, és átadja az `Ok`/`Mégse` parancsokat.
4. A `NavigationService.ShowCenteredDialog` megjeleníti a dialógust a főablak közepén. A `DialogHelper` gondoskodik a billentyűk leképezéséről.

### Tulajdonosi adatok felvétele

Első indításkor a `UserInfoWindow` kéri be a cég adatait. A mezők kötelezőek, a
`KeyboardManager` biztosítja, hogy `Enter` a következő mezőre lépjen. Az utolsó
mező után az `OK` gombra kerül a fókusz, majd megerősítő üzenet jelenik meg:
"Helyesek az adatok?". `Enter` elfogadja, `Escape` az előző mezőre visz
vissza. Minden üres mező piros keretet kap, amíg ki nem töltik.
