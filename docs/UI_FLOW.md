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

🧾 Invoice Editor Flow (Bejövő szállítólevelek)

1. Invoice Number Field (Lookup & Creation)

A ComboBox-like control at the top shows existing invoice numbers in descending date order

If user attempts to go above topmost item (0th row), an inline new invoice creation is triggered

Confirmation prompt: Create invoice XXXXX1231? (Enter=Yes, Esc=No)

2. Invoice Header Data

After invoice number confirmed:

Supplier selection (EditLookup)

Date selection (default = today, arrow or numpad)

Payment method (EditLookup)

Bruttó checkbox (affects unit price interpretation)

3. Invoice Line Items Entry

Focus shifts to the first line’s Product Name

EditLookup behavior with real-time filtering and keyboard navigation

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

📊 EditLookup UX-behavior

All master-data fields (e.g., Supplier, Product, TaxRate, Unit) use a unified EditLookup component:

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

The EditLookup behavior ensures consistent UX and keyboard flow across invoice fields.

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

┌─────────── Számla szerkesztő ─────────────────────────────────────┐
│ Szállító: [EditLookup   ]                   │
│ Dátum:    [2025-06-30  ]                   │
│ Szám:     [XXXXX1231   ]                   │
│ Fiz. mód: [EditLookup   ]   [ ] Bruttó     │
├────────────────────────────────────────────────┐
│ Termék       Menny. Term.csop. Me.e. Ár   ÁFA              │
│ ------------------------------------------│
│ [EditLookup ▼]  [  1  ] [EditLookup ▼] [EditLookup ▼] [100] [27% ▼]         │
│ ...added invoice items...                  │
│                                            │
│ Új sor felvitele automatikusan indul       │
├──────────────────────────────────────────────────┐
│ Negatív mennyiség = visszáru        │
└──────────────────────────────────────────────────┘

🔁 Special Behavior

All views must support full keyboard navigation

Inline creators must not shift focus away from the current context

Views are loaded in-place inside StageView, avoiding modal disruptions

Menu state persists across Escape presses to return user to most recent focus

📚 Future List Views

Menus will later populate listings (e.g., invoice history, product usage) from their respective modules

No need to implement grid-based listing yet; future enhancement

📌 Constraints

Archive logic must follow business rules (immutable once archived)

Bruttó flag controls pricing behavior throughout lifecycle

UX must reflect availability of actions based on current invoice state

ℹ️ This file is part of the coordinated documentation set along with BUSINESS_LOGIC.md and RefactorPlan.md. Use this UI Flow spec to align visual layout, keyboard handling, and interaction design with core logic and model behavior.
