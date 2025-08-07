# WinAppDriver UI tests

These tests exercise the WPF user interface using WinAppDriver.

## Setup

1. Install [WinAppDriver](https://github.com/microsoft/WinAppDriver).
2. Ensure the executable is available at `C:\\Program Files (x86)\\Windows Application Driver\\WinAppDriver.exe`.

## Running the tests

```
dotnet publish Wrecept.UI/Wrecept.UI.csproj -c Release -o build
dotnet test tests/Wrecept.UI.AutomatedTests/Wrecept.UI.AutomatedTests.csproj --no-build
```

The tests start WinAppDriver automatically and expect Windows.
