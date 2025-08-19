## Problem
No service existed to create invoices and compute VAT totals, and the TUI lacked a flow for entering new invoices.

## Approach
- Added `InvoiceService` with VAT snapshot and total calculations.
- Introduced `new_invoice` wizard using `DateField` and `EditFormController`.
- Exported the wizard from the TUI package.
- Verified totals for a single invoice line with a unit test.

## Files Changed
- `src/facturon_py/services/__init__.py`
- `src/facturon_py/services/invoice_service.py`
- `src/facturon_py/ui_tui/invoice_flow.py`
- `src/facturon_py/ui_tui/__init__.py`
- `tests/test_invoice_service.py`
- `COMMANDS.sh`
- `PR.txt`
- `LIMITS.txt`
- `SUMMARY.md`

## Risks & Mitigations
- Wizard uses simple text prompts; richer UI will come later.

## Assumptions
- VAT rates supplied via an in-memory table; persistence postponed.

