[CmdletBinding()]
param(
    [string]$Configuration = "Release"
)

$solution = Join-Path $PSScriptRoot "wrecept.sln"

if ($IsWindows) {
    dotnet build $solution -c $Configuration
    dotnet test $solution -c $Configuration
} else {
    dotnet build $solution -c NoUI
    dotnet test $solution -c NoUI --filter "Category!=UI"
}
