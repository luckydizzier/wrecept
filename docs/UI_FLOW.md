# UI Flow

## Overview

This document describes the workflow of the current Wrecept user interface. The application has been simplified to focus on invoice management, and currently the only screen implemented is an **Invoice Editor View**. Earlier versions mentioned a top menu and separate screens, but those features were removed during refactoring and are no longer part of the program.

## Invoice Editor View

When you launch Wrecept, the application opens directly into the Invoice Editor. The window is divided into two panes:

- **Invoice list (left pane)**: displays existing invoices. You can select an invoice to load its details or create a new one using the "New" command.
- **Editor area (right pane)**: contains entry fields for supplier, date, invoice number, payment method and a table for line items (product, quantity, unit, price, VAT and total). Action buttons allow you to add or remove lines and save the invoice.

```
|--------------- List ---------------| |----------- Invoice editor -----------|
| [Invoice number] Supplier: [____] | | Supplier:      [___________] [\u25bc] |
| [Date]            Date:    [____] | | Date:          [_____] [\u25bc]       |
| [Supplier]        Number:  [____] | | Payment method:[_____] [\u25bc]       |
|-------------------------------     | |--------------------------------------|
|                                     | | Product | Qty | Unit | Price | VAT |
|                                     | | [Add]    [  ] [   ] [     ] [   ] |
|                                     | | ... previously entered lines ...   |
|-------------------------------------| |--------------------------------------|
```

### Keyboard navigation

All actions in the Invoice Editor can be performed using the keyboard. Use the **Tab** key to move between input fields and **Enter** to confirm entries. Arrow keys navigate the invoice list on the left. The application respects standard WPF accessibility guidelines.

### Data persistence

Invoice data is saved to the local SQLite database when you click **Save**. Unsaved changes are highlighted in the title bar. Application settings are stored in `wrecept.json` (feature planned).

## Restrictions and future work

- Only the Invoice Editor is currently implemented. References to other dialogs such as a `UserInfoWindow` originate from outdated documentation and should be ignored.
- A top-level navigation menu and additional views (e.g., user info, settings) are planned for future releases. These will be documented once implemented.

For a broader overview of design principles and coding style, see `docs/styleguide.md`.
