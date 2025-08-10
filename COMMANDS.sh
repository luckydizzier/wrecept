#!/usr/bin/env bash
set -euo pipefail
dotnet build
dotnet test --logger "trx;LogFileName=test.trx"
# vagy: npm ci && npm run lint && npm test
