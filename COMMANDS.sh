#!/usr/bin/env bash
set -euo pipefail

ruff check src/facturon_py tests
black src/facturon_py tests
pytest
