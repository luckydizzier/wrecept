#!/usr/bin/env bash
set -euo pipefail
dotnet build wrecept.sln
dotnet test Wrecept.Core.Tests/Wrecept.Core.Tests.csproj
dotnet test tests/Wrecept.Domain.Tests/Wrecept.Domain.Tests.csproj
dotnet test Wrecept.UI.Tests/Wrecept.UI.Tests.csproj
dotnet test tests/Wrecept.UI.AutomatedTests/Wrecept.UI.AutomatedTests.csproj
dotnet list Wrecept.Core/Wrecept.Core.csproj package --outdated
