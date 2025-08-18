Problem: Active flag could be toggled without confirmation or audit trail, making accidental deactivation unrecoverable.
Approach: Added confirmation, audit logging, and undo support to EditFormController.
Files changed:
  - src/facturon_py/repo/audit_log.py
  - src/facturon_py/ui_tui/edit_views/edit_form.py
  - tests/test_edit_form_controller.py
Risks & mitigations:
  - Undo stack currently only tracks active flag; extend as needed for other fields.
Assumptions:
  - In-memory audit log suffices for tests; real persistence handled later.
