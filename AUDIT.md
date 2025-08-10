# Repository Audit

## 1. Repository Overview
Wrecept is an offline-first invoicing desktop application built with C# (.NET 8) and WPF using the MVVM pattern. The solution consists of:
- **Wrecept.Core** – business logic, models, services, repositories.
- **Wrecept.UI** – WPF UI consuming core services.
- **Wrecept.Domain** – domain entities/value objects shared via `src` folder.
- **Test projects** covering core and domain logic; UI and automation tests exist but require Windows.

## 2. Folder Structure
```text
.
├── AGENTS.md — repository rules
├── README.md — project overview
├── docs/ — style guide, install docs, UX notes, progress logs
├── Wrecept.Core/ — core business logic (Data, Models, Services, Repositories)
├── Wrecept.Core.Tests/ — xUnit tests for core layer
├── Wrecept.UI/ — WPF UI (App.xaml, ViewModels, Views, Themes)
├── Wrecept.UI.Tests/ — WPF UI unit tests (Windows-only)
├── src/
│   └── Wrecept.Domain/ — domain entities and value objects
├── tests/
│   ├── Wrecept.Domain.Tests/ — tests for domain entities
│   └── Wrecept.UI.AutomatedTests/ — Appium-based UI automation tests
├── build.ps1 — PowerShell build script
├── wrecept.sln — solution file
├── Wrecept.Core.sln — solution focusing on core library
└── TODO.md — task tracker
```

## 3. Findings
### Code
- Modular separation between core, UI, and domain modules.
- Limited exception handling and input validation in services.
- Minimal comments and documentation within code files.

### Tests
- `dotnet test Wrecept.Core.Tests` passed 12 tests
- `dotnet test tests/Wrecept.Domain.Tests` passed 16 tests
- `dotnet test Wrecept.UI.Tests` failed: missing WindowsDesktop targets
- `dotnet test tests/Wrecept.UI.AutomatedTests` executed but discovered zero tests
- Building entire solution failed due to missing WindowsDesktop SDK

### Documentation
- Comprehensive README and style guide.
- Additional docs for installation, UX, and progress logs.
- Some planned features marked but not yet documented; progress logs may be sparse.

### Dependencies
- Core project uses `Microsoft.EntityFrameworkCore` and `Microsoft.Extensions.Hosting` 8.0.0; newer 9.0.8 available
- UI project uses Serilog packages; version checks skipped due to missing WindowsDesktop SDK.
- `Appium.WebDriver` pinned to 4.4.5 with a compatibility warning, posing potential security lag.

### Security
- No credentials or secrets found via repository search.
- Config file `wrecept.json` contains only non-sensitive settings

## 4. Recommendations
- Configure WindowsDesktop SDK or adjust solution to allow cross-platform builds.
- Add robust error handling and input validation in services and view models.
- Increase test coverage for UI; ensure automated tests execute meaningful cases.
- Update outdated packages where possible and monitor pinned dependencies.
- Expand progress logs and architecture documentation to stay current.

## 5. Execution-Ready Task List
### Milestone 1 – Build Pipeline
- **Title**: Enable cross-platform build  
- **Description**: Install WindowsDesktop SDK or adjust solution to skip Windows projects on non-Windows hosts.  
- **Required files/folders**: `Wrecept.UI`, `Wrecept.UI.Tests`, `.github/workflows/ci.yml`  
- **Dependencies**: None  
- **Risk level**: Medium  

### Milestone 2 – Testing Expansion
- **Title**: Restore UI tests  
- **Description**: Ensure UI tests compile and run; add meaningful Appium tests.  
- **Required files/folders**: `Wrecept.UI.Tests`, `tests/Wrecept.UI.AutomatedTests`  
- **Dependencies**: Milestone 1  
- **Risk level**: High  

### Milestone 3 – Dependency Updates
- **Title**: Update core package versions  
- **Description**: Upgrade EF Core and Microsoft.Extensions.Hosting to latest stable.  
- **Required files/folders**: `Wrecept.Core/Wrecept.Core.csproj`  
- **Dependencies**: Milestone 1 (build needs to succeed)  
- **Risk level**: Low  

### Milestone 4 – Code Quality
- **Title**: Improve error handling  
- **Description**: Add validation and exception handling in services and view models.  
- **Required files/folders**: `Wrecept.Core/Services`, `Wrecept.UI/ViewModels`  
- **Dependencies**: None  
- **Risk level**: Medium  

### Milestone 5 – Documentation
- **Title**: Enhance project docs  
- **Description**: Update progress logs and document architecture decisions.  
- **Required files/folders**: `docs/progress`, `docs/**`  
- **Dependencies**: None  
- **Risk level**: Low  

## 6. Questions/Clarifications Needed
None.

