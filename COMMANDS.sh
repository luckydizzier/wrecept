#!/usr/bin/env bash
set -euo pipefail

pip install -r src/facturon_py/requirements.txt
ruff check src/facturon_py tests
black src/facturon_py tests
pytest
