Problem: The repository lacked a Python scaffold for the Facturon-Py MVP.
Approach: Added base package, requirements, and a smoke test to establish tooling.
Files changed:
  - src/facturon_py/__init__.py
  - src/facturon_py/AGENTS.md
  - src/facturon_py/README.md
  - src/facturon_py/requirements.txt
  - tests/test_environment.py
Risks & mitigations:
  - Mixing Python and .NET projects could confuse tooling; kept files in isolated paths.
Assumptions:
  - Output contract files are exempt from the 5-file limit.
