# 🚨 Critical Issues Summary

> **Immediate Action Required**

## 🔴 Build Blockers

1. **WindowsDesktop SDK Missing**
   - **Impact**: Cannot build WPF projects (Wrecept.Wpf, Wrecept.UiTests) on Linux
   - **Error**: `Microsoft.NET.Sdk.WindowsDesktop.targets not found`
   - **Next Step**: Install .NET Desktop runtime or configure Windows build environment

2. **Project Compatibility**
   - **Impact**: `Wrecept.Tests` cannot reference WPF projects
   - **Error**: `Project Wrecept.Wpf is not compatible with net8.0`
   - **Next Step**: Adjust target framework or split test projects

## 🟡 Test Infrastructure

3. **UI Test Failures (6/14 failing)**
   - **Impact**: Cannot verify UI functionality
   - **Status**: StageViewFocusUITests and StartupWindowTests partially fixed
   - **Next Step**: Investigate remaining 4 failing tests

4. **Linux Test Execution**
   - **Impact**: Cannot run UI tests in CI/CD
   - **Cause**: WPF dependencies unavailable on Linux
   - **Next Step**: Consider Windows agents or headless alternatives

## 🟢 Development Workflow

5. **Missing Using Directives**
   - **Files**: `ProductGroupMasterViewModel`, `TaxRateMasterViewModel`
   - **Impact**: Compilation warnings/errors
   - **Fix**: Add `using System.Threading.Tasks;`

---

**Priority**: Address items 1-2 first to restore full build capability, then tackle test issues.