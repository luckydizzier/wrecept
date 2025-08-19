## Problem
Date inputs lacked validation and locale-aware prompts, allowing invalid or ambiguous entries.

## Approach
- Added `DateField` widget with locale-based masks and normalization.
- Provided validator for prompt_toolkit editors.
- Covered parsing and prompts in unit tests.

## Files Changed
- `src/facturon_py/ui_tui/widgets/date_field.py`
- `src/facturon_py/ui_tui/widgets/__init__.py`
- `tests/test_date_field.py`
- `COMMANDS.sh`
- `PR.txt`
- `LIMITS.txt`
- `SUMMARY.md`

## Risks & Mitigations
- **Locale coverage** limited to few locales → fallback to ISO format.

## Assumptions
- Using regex + strptime is sufficient for mask enforcement.
