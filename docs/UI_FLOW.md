# UI Flow

<!-- markdownlint-disable MD013 -->

## 🧱 Overview

This document describes the workflow of the Wrecept user interface. It details the
navigation model, expected behaviors, and data entry steps, in line with inline
editing support.

## 📌 Navigation model

Navigation is strictly keyboard-based. Press `Enter` to move forward or confirm,
and `Escape` to move back or cancel. Mouse input and the `Tab` key are not used.

## 📀 Screen layouts

### 🔳 Main menu flow

┌──────────────────────────────────────────────────────┐
│ [Accounts] [Stocks] [Lists] [Maintenance] [Contacts] │
│                                                      │
│ > Incoming delivery notes                            │
└──────────────────────────────────────────────────────┘

### 🧾 Invoice editor view

┌───── List ────┬──────── Invoice editor ───────────────┐
│ [Invoice number] │ Supplier: [              ]        │
│ [Date]           │ Number:   [              ]        │
│ [Supplier]       │ Payment method: [        ][ ] Net │
│                  ├───────────────────────────────────┤
│                  │ Product  Qty. Group Unit price VAT│
│                  │ [Edit] [ 1] ...                   │
│                  │ ... Previously entered lines ...  │
│                  │                                   │
└──────────────────┴───────────────────────────────────┘

## Invoice entry workflow

1. From the main menu, start a new invoice with `Enter`.
2. `Invoice No` field is focused; type the value and press `Enter`.
3. `Date` accepts manual input; press `Enter` to continue.
4. In `Supplier`, type letters and press `Enter` to accept the first match.
5. `Customer ID` auto-fills; press `Enter` to proceed to the item grid.
6. For each item row:
   - Enter item code and press `Enter`.
   - Description fills automatically; press `Enter`.
   - Enter quantity, then `Enter`.
   - Enter unit price, then `Enter`.
   - Tax rate is selected automatically; press `Enter` to accept.
   - Press `Enter` at the end of the row to add a new one.
   - Press `Escape` on an empty row to remove it.
7. Move to `[Save]` and press `Enter` to store the invoice; `Escape` cancels.
8. After saving, focus returns to the invoice list.

## 📌 Restrictions

- Archiving can only be done according to business rules and cannot be modified
  afterwards.
- The Gross designation determines the pricing throughout.
- The interface must always reflect which operations are available in the current
  state.

### Recording owner data

When first launched, `UserInfoWindow` requests the company details. The fields are
mandatory. After the last field, the focus moves to the `OK` button and a
confirmation message appears: "Are the details correct?". `Enter` accepts,
`Escape` returns to the previous field. All required fields are highlighted in red
until they are filled in. The program checks the database for the entry and
creates it after confirmation if it does not exist.

For later modifications, the *Service / Edit owner...* menu item opens the same
`UserInfoWindow` dialog.
