UI_FLOW.md

🧱 Overview

This document describes the workflow of the Wrecept user interface. It details the navigation model, expected behaviors, and data entry steps, in line with inline editing support.

📌 Navigation model

When launched, an empty screen appears with the Accounts menu item selected in the top menu bar.

📀 Screen layouts

🔳 Main menu flow

┌──────────────────────────────────────────────────────┐
│ [Accounts] [Stocks] [Lists] [Maintenance] [Contacts]│
│                                                      │
│ > Incoming delivery notes │
└──────────────────────────────────────────────────────┘

🧾 Invoice editor view

┌───── List ────┬──────── Invoice editor ───────────────┐
│ [Invoice number] │ Supplier: [              ] │
│ [Date]        │ Date:    [2025-08-04  ] │
│ [Supplier] │ Number: [              ] │
│                │ Payment method: [            ][ ] Net invoice │
│                ├──────────────────────────────────────────┤
│                │ Product  Qty. Group Unit price  VAT │
│                │ [Edit] [  1] ... │
│                │ ... Previously entered lines ... │
│                │                                            │
└────────────────┴──────────────────────────────────────────┘

📌 Restrictions

- Archiving can only be done according to business rules and cannot be modified afterwards.
- The Gross designation determines the pricing throughout.
- The interface must always reflect which operations are available in the current state.
### Recording owner data

When first launched, `UserInfoWindow` requests the company details. The fields are mandatory, after the
last
field the focus moves to the `OK` button and a confirmation message appears:
"Are the details correct?". `Enter` accepts, `Escape` takes you back to the previous field
. All required fields are highlighted in red until they are filled in.
It then checks the database for the entry and, if it does not exist, creates it after confirmation.

For later modifications, the *Service / Edit owner...* menu item opens the same
`UserInfoWindow` dialog.
