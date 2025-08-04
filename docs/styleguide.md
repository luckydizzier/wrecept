# Style Guide for `wrecept`

This style guide defines the standards for code, documentation, and UI design used throughout the `wrecept` project.
All contributors must follow these conventions to ensure consistency, readability, and maintainability across the codebase.

---

## 📁 Project Structure

```
wrecept/
├── Wrecept.WpfApp/           # WPF application
│   ├── App/                  # Application entry and configuration
│   │   ├── App.xaml
│   │   └── App.xaml.cs
│   ├── Core/                 # Core business logic and models
│   │   ├── Models/
│   │   ├── Services/
│   │   └── Core.csproj
│   ├── Data/                 # Data access and repositories
│   │   ├── Repositories/
│   │   ├── Entities/
│   │   └── Data.csproj
│   ├── Infrastructure/       # External integrations and utilities
│   │   ├── Logging/
│   │   ├── Email/
│   │   └── Infrastructure.csproj
│   ├── Agents/               # Agent logic and instructions
│   │   ├── InvoiceAgent.cs
│   │   └── SupplierAgent.cs
│   ├── Tests/                # Unit and integration tests
│   │   ├── Core.Tests/
│   │   ├── Data.Tests/
│   │   └── Infrastructure.Tests/
│   └── Wrecept.WpfApp.csproj
├── docs/                     # Project documentation
├── build.ps1                 # Build script
├── wrecept.sln               # Solution file
├── README.md                 # Project overview
├── TODO.md                   # Task tracker (updated per workflow)
└── AGENTS.md                 # Agent instructions
```

---

## 🧑‍💻 Coding Standards

### C# Language

* **Target Framework:** .NET 8.0
* **Nullable Context:** Enabled
* **File Encoding:** UTF-8 without BOM

### Naming Conventions

| Element           | Convention              | Example                             |
| ----------------- | ----------------------- | ----------------------------------- |
| Namespace         | PascalCase              | `Wrecept.Core.Models`               |
| Class / Interface | PascalCase              | `InvoiceService`, `IUnitRepository` |
| Method            | PascalCase              | `GetInvoicesByDate()`               |
| Variable / Field  | camelCase               | `invoiceId`, `_logger`              |
| Constant          | PascalCase with `const` | `const int MaxRetryCount`           |
| XAML Fields       | Hungarian + CamelCase   | `txtSupplierName`, `lblTotal`       |

### File Structure

* One public type per file
* File name must match the type name
* Keep regions minimal (`#region` discouraged unless aiding navigation)

---

## 📐 Architecture Guidelines

### Design Patterns

* MVVM for UI layer
* MRS (Model-Repository-Service) for domain logic
* Repository pattern for database abstraction
* Dependency Injection via constructor and `StartupOrchestrator`

### Folder Layout by Responsibility

* `ViewModels/` → only UI-binding logic
* `Models/` → pure data + validation
* `Services/` → business operations
* `Repositories/` → CRUD + query logic

---

## 🪟 WPF + XAML Guidelines

### General

* All UI must be keyboard navigable
* Use `Style` and `ControlTemplate` for consistent theming
* Avoid hardcoded margins, sizes; prefer `DynamicResource` or `Grid`-based layout

### Control Naming (Hungarian prefix)

| Control Type | Prefix | Example            |
| ------------ | ------ | ------------------ |
| TextBox      | `txt`  | `txtSupplierName`  |
| Label        | `lbl`  | `lblTotalAmount`   |
| Button       | `btn`  | `btnSaveInvoice`   |
| ComboBox     | `cmb`  | `cmbPaymentMethod` |
| DataGrid     | `dg`   | `dgInvoiceItems`   |

### Binding

* Use `INotifyPropertyChanged` and `{Binding}`
* Avoid code-behind unless absolutely necessary (use `ICommand`)

---

## 🧪 Unit Testing

* Framework: xUnit
* Use `Tests/` mirror structure of main app
* Test filenames: `<Type>Tests.cs`
* Mock external dependencies using Moq or custom test doubles
* Follow AAA pattern (Arrange-Act-Assert)

---

## 🧾 Documentation Conventions

### Markdown Files

* Stored in `docs/`
* Filename format: `lowercase-hyphenated.md`
* Use `#` headers for structure, `---` for separation
* Code samples must be fenced blocks with language identifier

### Progress Logs

* Path: `docs/progress/YYYY-MM-DD_HH-MM-SS_agentname.md`
* Log every non-trivial action, referencing tasks or milestones

### Task Tracker

* Path: `TODO.md`
* Use these statuses: `TODO`, `IN_PROGRESS`, `DONE`, `NEEDS_HUMAN_DECISION`
* Always reference task ID in commits and logs

---

## 📦 Git & Commit Conventions

### Branches

* Main branch: `main`
* Feature branches: `feature/<short-description>`
* Fix branches: `fix/<issue>`

### Commits

* Small, atomic
* Prefix with agent or scope
* Reference task or milestone

**Example:**

```
[core] Implemented invoice total recalculation logic
Ref: TODO #42, Milestone: stage-3
```

---

## 📋 UI/UX Workflow Rules

* No console output allowed in production builds
* All messages must be in **Hungarian**, and shown in dialog if user-facing
* Use Enter/Esc navigation for confirmation dialogs
* Do not block UI thread (use async where needed)

---

## 📎 Configuration and Settings

* Store all user-facing settings in `config.json`
* Validate at startup, with GUI warning if missing or invalid

---

## 📣 Logging (Serilog)

* Output: `logs/wrecept-yyyy-MM-dd.log`
* Use `Information`, `Warning`, `Error` ,`Debug` levels appropriately
* Configure via `appsettings.json` or in DI at startup

---

## 🧠 Final Notes

This guide will evolve with the project. If in doubt, prefer:

* **Readability over cleverness**
* **Consistency over personal style**
* **Structure over speed**

Please report style violations or ambiguities to the `root_agent` for review.

Happy coding! 🎯
