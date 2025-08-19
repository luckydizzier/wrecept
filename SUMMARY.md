## Problem
No centralized shortcut registry and F1 key lacked help popup, making keyboard hints inconsistent.

## Approach
- Introduced `shortcuts` registry with dialog helper.
- Registered edit-form shortcuts and mapped F1 to show them.
- Covered registry and F1 handler with tests.

## Files Changed
- `src/facturon_py/ui_tui/shortcuts.py`
- `src/facturon_py/ui_tui/edit_views/edit_form.py`
- `tests/test_edit_form_controller.py`
- `tests/test_shortcuts.py`
- `PR.txt`
- `LIMITS.txt`
- `SUMMARY.md`

## Risks & Mitigations
- Dialog text hardcoded in Hungarian → adjust in registry if localization changes.

## Assumptions
- Displayed key names (e.g., "Esc") are sufficient for user understanding.
