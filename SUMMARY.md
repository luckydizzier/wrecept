## Problem
No main menu existed, so users couldn't navigate via keyboard or view shortcut hints.

## Approach
- Added prompt_toolkit main menu with Alt+number navigation.
- Registered F1 in shortcut registry to display available keys.
- Exposed `run_main_menu()` and covered selection/exit via simulated key presses.

## Files Changed
- `src/facturon_py/ui_tui/main_menu.py`
- `src/facturon_py/ui_tui/__init__.py`
- `tests/test_main_menu.py`
- `COMMANDS.sh`
- `PR.txt`
- `LIMITS.txt`
- `SUMMARY.md`

## Risks & Mitigations
- Menu texts hardcoded in Hungarian → refactor for localization later.

## Assumptions
- Alt+digit mapping is adequate for initial navigation.
