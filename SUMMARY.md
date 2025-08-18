Problem: Exiting edit mode with unsaved changes provided no confirmation, risking accidental data loss.
Approach: Introduced an EditFormController that tracks dirty fields and prompts on Esc, with tests exercising confirm and decline flows.
Files changed:
  - src/facturon_py/ui_tui/__init__.py
  - src/facturon_py/ui_tui/edit_views/__init__.py
  - src/facturon_py/ui_tui/edit_views/edit_form.py
  - tests/test_edit_form_controller.py
Risks & mitigations:
  - Prompt logic could block automation; injectable confirm callable allows headless tests.
Assumptions:
  - Esc should exit immediately when no fields are dirty.
