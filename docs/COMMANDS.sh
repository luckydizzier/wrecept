#!/usr/bin/env bash
set -euo pipefail

# Build core project always
if dotnet workload list | grep -q windowsdesktop; then
  echo "windowsdesktop workload present - building full solution"
  dotnet build wrecept.sln
  dotnet test Wrecept.Core.Tests/Wrecept.Core.Tests.csproj
  dotnet test Wrecept.UI.Tests/Wrecept.UI.Tests.csproj
else
  echo "windowsdesktop workload missing - building core only"
  dotnet build Wrecept.Core/Wrecept.Core.csproj
  dotnet test Wrecept.Core.Tests/Wrecept.Core.Tests.csproj
  echo "Skipping UI tests"
fi
