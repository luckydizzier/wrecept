<#
.SYNOPSIS
Builds the Wrecept WPF application as a self-contained executable.

.DESCRIPTION
Runs dotnet publish for the Wrecept.WpfApp project to produce a self-contained, single-file Windows executable. Execute from the repository root.

.PARAMETER Configuration
Build configuration. Defaults to 'Release'.

.PARAMETER Runtime
Target runtime identifier (RID). Defaults to 'win-x64'.

.PARAMETER Output
Output directory for published files. Defaults to 'publish'.

.EXAMPLE
pwsh -File build.ps1 -Configuration Release -Runtime win-x64 -Output publish
#>

[CmdletBinding()]
param(
    [string]$Configuration = "Release",
    [string]$Runtime = "win-x64",
    [string]$Output = "publish"
)

$project = Join-Path $PSScriptRoot "Wrecept.WpfApp/Wrecept.WpfApp.csproj"

if (-not (Test-Path $project)) {
    throw "Project file not found: $project"
}
$publishArgs = @(
    $project,
    '-c', $Configuration,
    '-r', $Runtime,
    '--self-contained', 'true',
    '/p:PublishSingleFile=true',
    '-o', $Output
)

dotnet publish @publishArgs
if ($LASTEXITCODE -ne 0) {
    throw "dotnet publish failed with exit code $LASTEXITCODE"
}
