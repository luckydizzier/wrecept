# Execution-Ready Task List

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
- **Dependencies**: Milestone 1
- **Risk level**: High

### Milestone 3 – Dependency Updates
- **Title**: Update core package versions
- **Description**: Upgrade EF Core and Microsoft.Extensions.Hosting to latest stable.
- **Required files/folders**: `Wrecept.Core/Wrecept.Core.csproj`
- **Dependencies**: Milestone 1 (build needs to succeed)
- **Risk level**: Low

### Milestone 4 – Code Quality
- **Title**: Improve error handling
- **Description**: Add validation and exception handling in services and view models.
- **Required files/folders**: `Wrecept.Core/Services`, `Wrecept.UI/ViewModels`
- **Dependencies**: None
- **Risk level**: Medium

### Milestone 5 – Documentation
- **Status**: DONE
- **Title**: Enhance project docs
- **Description**: Update progress logs and document architecture decisions.
- **Required files/folders**: `docs/progress`, `docs/**`
- **Dependencies**: None
- **Risk level**: Low
