#!/usr/bin/env bash
set -euo pipefail

dotnet build wrecept.sln
dotnet test wrecept.sln
