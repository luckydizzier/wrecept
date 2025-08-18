#!/usr/bin/env bash
set -euo pipefail

ruff check src/facturon_py tests
black src/facturon_py tests/test_environment.py
pytest tests/test_environment.py
