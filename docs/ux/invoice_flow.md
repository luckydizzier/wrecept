# Invoice Flow UX Analysis

## Grid-Based Layout Proposal
To minimize eye movement and support rapid data entry, the invoice form uses a two-column grid where related fields align horizontally. This keeps the user's focus in a narrow vertical band and reduces scanning time.

```
┌───────────────────────┬────────────────────────────────────────┐
│ Számlák               │ Számlaszerkesztő                       │
│ > INV-2025-101        │ Szállító:      [            ]          │
│ INV-2025-102          │ Dátum:         [ 2025-08-15 ]          │
│ INV-2025-103          │ Számlaszám:    [ INV-2025-101 ]        │
│ [Új számla]           │ Esedékesség:   [ 2025-09-14 ]          │
│                       │ Fizetési mód:  [ Banki átutalás ]      │
│                       ├────────────────────────────────────────┤
│                       │ Kód  Leírás     Mennyiség  Ár   Összesen│
│                       │ > [ ] [ ]       [ ]        [ ]  [ ]     │
│                       │   [ ] [ ]       [ ]        [ ]  [ ]     │
│                       │                                  Nettó: │
│                       │                                  ÁFA:   │
│                       │                                  Bruttó:│
│                       ├────────────────────────────────────────┤
│                       │ [Elem hozzáadása] [Elem eltávolítása]  │
│                       │                         [Mentés]       │
└───────────────────────┴────────────────────────────────────────┘
```

The left panel lists existing invoices while the right panel hosts the editor. At the top of the editor a two-column grid aligns related header fields horizontally. Below, an item grid behaves like a spreadsheet for fast keyboard entry. A dedicated action bar at the bottom separates data entry from commands such as **Elem hozzáadása**, **Elem eltávolítása**, and **Mentés**. Pressing `Enter` after the last item row moves focus to this bar; `Enter` cycles forward through its buttons while `Escape` steps back or returns to the grid.

## Keyboard-Only Workflow Simulation
1. **Start new invoice**: `Ctrl+N`
2. **Invoice No** field is focused; type value and press `Enter` to move forward
3. **Date**: type date manually, `Enter`
4. **Customer**: type name, use `Arrow` keys for suggestions, `Enter` to select
5. **Customer ID** auto-fills; press `Enter` to reach the item grid
6. **Item grid**:
    - Type item code, `Enter`
    - Description auto-fills; `Enter`
    - Enter quantity, `Enter`
    - Enter unit price, `Enter`
    - Select tax rate with `Arrow` keys, `Enter`
    - Line total calculates automatically
    - Press `Enter` at end of row to add a new one; on the last empty row `Enter` moves focus to the action bar
    - Press `Escape` on an empty row to remove it or move back
7. **Action bar**: `Enter` cycles `[Elem hozzáadása] → [Elem eltávolítása] → [Mentés]`; `Escape` moves backward or returns to the grid
8. **Save**: press `Enter` on `[Mentés]`; `Escape` cancels
9. **Confirm** dialog: `Enter`
10. **Return**: focus goes back to the invoice list

## Friction Points & Improvements
- **Date entry requires mouse for calendar** → Allow direct date typing and `Alt+Down` to open calendar.
- **Customer search lacks keyboard shortcuts for filtering** → Add type-ahead search and `Ctrl+L` to focus customer field.
- **Adding items requires reaching for mouse to add rows** → Implement `Enter` to append new row and `Ctrl+Delete` to remove.
- **Focus can get lost after saving** → After save, return focus to invoice list for continued keyboard navigation.
- **Lack of inline help** → Provide contextual tooltips directly within the interface for quick guidance.

*Prepared by ux_agent*
