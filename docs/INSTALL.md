# Installation Guide

Follow these steps to set up **Wrecept** on your machine.

## Prerequisites

- Windows 10 or later
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- Git
- [PowerShell 7](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell) (optional, for build script)
- Visual Studio 2022 or later (optional)

## Build and Run

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/wrecept.git
   cd wrecept
   ```
2. Restore dependencies:
   ```bash
   dotnet restore
   ```
3. Build the solution:
   ```bash
   dotnet build
   ```
4. Run the application:
   ```bash
   dotnet run --project Wrecept.WpfApp
   ```

## Publish a Self-contained Build

To create a standalone Windows executable:

```powershell
pwsh -File build.ps1 -Configuration Release -Runtime win-x64 -Output publish
```

The published files will appear in the specified output directory.

---
