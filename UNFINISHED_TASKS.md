# 🔄 Unfinished Tasks - Wrecept Project

> **Last Updated:** 2025-07-05  
> **Source:** Analysis of progress reports, test results, and feature planning documents

This document compiles all unfinished tasks across the Wrecept project, categorized by type and priority.

---

## 🔧 Build & Environment Issues

### **Critical - Prevents Full Build**
- [ ] **WindowsDesktop SDK Missing**: Build fails with `Microsoft.NET.Sdk.WindowsDesktop.targets` not found
  - **Impact**: WPF projects (Wrecept.Wpf, Wrecept.UiTests) cannot build on Linux
  - **Agent**: `root_agent` / Infrastructure
  - **Source**: `docs/progress/2025-06-30_22-40-52_code_agent.md`

- [ ] **Project Compatibility**: `Wrecept.Tests` not compatible with net8.0 
  - **Impact**: Test project cannot reference WPF projects
  - **Agent**: `code_agent`
  - **Source**: Build output analysis

### **Medium Priority**
- [ ] **Missing Using Directives**: `using System.Threading.Tasks` needed in ViewModels
  - **Files**: `ProductGroupMasterViewModel`, `TaxRateMasterViewModel`
  - **Agent**: `code_agent`
  - **Source**: `docs/progress/2025-06-30_12-28-17_code_agent.md`

---

## 🧪 Test Failures

### **UI Test Failures (6 out of 14 failing)**
- [ ] **StageViewFocusUITests**: Focus handling issues in UI navigation
  - **Status**: Partially fixed - now checks active menu item `Name` attribute
  - **Agent**: `test_agent`
  - **Source**: `docs/progress/2025-07-04_21-53-41_docs_agent.md`

- [ ] **StartupWindowTests**: Test data cleanup and window handling
  - **Status**: `TestHelper.PrepareSettings` clears test data for consistency
  - **Agent**: `test_agent`
  - **Source**: `docs/progress/2025-07-04_21-53-41_docs_agent.md`

- [ ] **Linux UI Test Execution**: WPF dependencies prevent UI tests on Linux
  - **Impact**: Cannot run UI tests in CI/CD pipeline
  - **Agent**: `test_agent`
  - **Source**: `docs/progress/2025-07-04_19-14-51_test_agent.md`

### **Test Infrastructure**
- [ ] **Test Path Resolution**: UI tests need relative path computation for `Wrecept.Wpf.exe`
  - **Status**: Partially completed
  - **Agent**: `test_agent`
  - **Source**: `docs/progress/2025-07-04_12-53-37_test_agent.md`

---

## 🎯 Planned Features (From FEATURE_PLAN.md)

### **Payment Method Entity - PLANNED**
- [ ] **Core Model**: Define `PaymentMethod` model with `Id`, `Name`
  - **Agent**: `core_agent`
  - **Blocking**: None

- [ ] **Database Layer**: Add EF migration, create seed data (Cash, Card, Transfer)
  - **Agent**: `storage_agent`
  - **Blocking**: `core_agent` completion

- [ ] **ViewModel Layer**: Scaffold `PaymentMethodViewModel`, bindable list
  - **Agent**: `code_agent`
  - **Blocking**: `storage_agent` completion

- [ ] **UI Layer**: Create dropdown in InvoiceEditor
  - **Agent**: `ui_agent`
  - **Blocking**: `code_agent` completion

- [ ] **Navigation Logic**: Wire Enter/Esc handling, update selection logic
  - **Agent**: `logic_agent`
  - **Blocking**: `ui_agent` completion

### **Keyboard-Only Lookup Workflow - PLANNED**
- [ ] **SmartLookup UI Pattern**: Implement keyboard-only product/supplier lookup
  - **Agent**: `ui_agent`
  - **Blocking**: None

- [ ] **Navigation Handling**: Handle navigation, trigger Add dialog
  - **Agent**: `logic_agent`
  - **Blocking**: `ui_agent` completion

- [ ] **Async Service Support**: Ensure services support async create & reload
  - **Agent**: `core_agent`
  - **Blocking**: `logic_agent` completion

- [ ] **ViewModel Binding**: ViewModel binding and error handling
  - **Agent**: `code_agent`
  - **Blocking**: `core_agent` completion

- [ ] **User Feedback**: Show success/failure banners and sounds
  - **Agent**: `feedback_agent`
  - **Blocking**: `code_agent` completion

---

## 🎨 UI & UX Issues

### **Focus Management**
- [ ] **Focus Handling**: ProgressWindow closure focus request with `Dispatcher.ContextIdle` priority
  - **Status**: Recently addressed but may need testing
  - **Agent**: `logic_agent`
  - **Source**: `docs/progress/2025-07-05_01-58-47_logic_agent.md`

- [ ] **Inline Creator Focus**: Only request focus when visible
  - **Status**: Recently addressed but may need testing
  - **Agent**: `logic_agent`
  - **Source**: `docs/progress/2025-07-05_01-58-47_logic_agent.md`

### **Input Validation**
- [ ] **UserInfo Validation**: Red border for invalid fields, mandatory field completion
  - **Status**: Recently completed but may need testing
  - **Agent**: `ui_agent`
  - **Source**: `docs/progress/2025-07-03_23-05-46_code_agent.md`

---

## 🐛 Code Quality Issues

### **Compiler Warnings**
- [ ] **CS1998 Warnings**: Async methods without await
  - **Status**: Some fixed by simplifying to synchronous Task
  - **Agent**: `code_agent`
  - **Source**: `docs/progress/2025-06-30_22-40-52_code_agent.md`

- [ ] **CS4014 Warnings**: Suppressed via `Dispatcher.InvokeAsync` discard
  - **Status**: Recently addressed
  - **Agent**: `code_agent`
  - **Source**: `docs/progress/2025-07-05_02-09-15_code_agent.md`

### **Design-Time Issues**
- [ ] **Service Initialization**: Fixed design-time service initialization in inline creator views
  - **Status**: Recently completed
  - **Agent**: `code_agent`
  - **Source**: `docs/progress/2025-07-05_02-09-15_code_agent.md`

---

## 📊 Milestone 1 Deliverables (From TASKLOG.md)

### **Core UI Components**
- [ ] **Visual-only Main Menu**: Layout stub (`StageView`) completion
  - **Agent**: `ui_agent`
  - **Status**: Partially complete

- [ ] **InvoiceEditor View**: Mocked-up item rows
  - **Agent**: `ui_agent` + `logic_agent`
  - **Status**: Partially complete

- [ ] **Product Search Views**: Basic product and supplier search
  - **Agent**: `ui_agent` + `core_agent`
  - **Status**: Partially complete

- [ ] **Keyboard Routing**: Full keyboard routing and error suppression
  - **Agent**: `logic_agent` + `feedback_agent`
  - **Status**: In progress

---

## 🔍 Documentation Gaps

### **Missing Documentation**
- [ ] **Error Handling Documentation**: Update based on recent fixes
  - **Agent**: `docs_agent`
  - **Source**: Multiple recent progress reports

- [ ] **KeyboardFlow Documentation**: Update with recent focus handling changes
  - **Agent**: `docs_agent`
  - **Source**: `docs/progress/2025-07-05_01-58-47_docs_agent.md`

---

## 📋 Task Priority Legend

- **🔴 Critical**: Blocks build or core functionality
- **🟡 High**: Impacts user experience or development workflow
- **🟢 Medium**: Quality improvements or non-blocking enhancements
- **🔵 Low**: Nice-to-have features or optimizations

---

## 🎯 Next Steps Recommendation

1. **Fix Build Issues**: Address WindowsDesktop SDK and project compatibility
2. **Stabilize UI Tests**: Fix the 6 failing tests and Linux execution
3. **Complete Payment Method Feature**: Follow the planned agent sequence
4. **Implement Keyboard-Only Lookup**: Major UX enhancement
5. **Documentation Updates**: Reflect recent changes and current state

---

*This document is generated from analysis of progress reports, feature plans, and current repository state. For specific implementation details, refer to the individual progress reports in `docs/progress/`.*