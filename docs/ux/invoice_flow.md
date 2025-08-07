# Invoice Flow UX Analysis

## Grid-Based Layout Proposal
To minimize eye movement and support rapid data entry, the invoice form uses a two-column grid where related fields align horizontally. This keeps the user's focus in a narrow vertical band and reduces scanning time.

```
+----------------+----------------+
| Invoice No     | Date           |
+----------------+----------------+
| Customer       | Customer ID    |
+----------------+----------------+
+-----------+-------------+-----+------------+-----+------------+
| Item Code | Description | Qty | Unit Price | Tax | Line Total |
+-----------+-------------+-----+------------+-----+------------+
| [Add Item] [Remove Item]                       [Save] [Cancel]|
+---------------------------------------------------------------+
```

## Keyboard-Only Workflow Simulation
1. **Start new invoice**: `Ctrl+N`
2. **Invoice No** field is focused; type value and press `Tab`
3. **Date**: type date manually, `Tab`
4. **Customer**: type name, use `Arrow` keys for suggestions, `Enter` to select, `Tab`
5. **Customer ID** auto-fills; `Tab`
6. **Item grid**:
   - Type item code, `Tab`
   - Description auto-fills; `Tab`
   - Enter quantity, `Tab`
   - Enter unit price, `Tab`
   - Select tax rate with `Arrow` keys, `Tab`
   - Line total calculates automatically
   - Press `Ctrl+Enter` to add new row and repeat
7. **Save**: `Alt+S`
8. **Confirm** dialog: `Enter`

## Friction Points & Improvements
- **Date entry requires mouse for calendar** → Allow direct date typing and `Alt+Down` to open calendar.
- **Customer search lacks keyboard shortcuts for filtering** → Add type-ahead search and `Ctrl+L` to focus customer field.
- **Adding items requires reaching for mouse to add rows** → Implement `Ctrl+Enter` to add new row and `Ctrl+Delete` to remove.
- **Focus can get lost after saving** → After save, return focus to invoice list for continued keyboard navigation.

*Prepared by ux_agent*
