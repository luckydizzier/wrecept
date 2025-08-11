#!/usr/bin/env bash
set -euo pipefail

dotnet build Wrecept.Core.sln
dotnet test Wrecept.Core.Tests
dotnet test tests/Wrecept.Domain.Tests
