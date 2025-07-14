---
title: "InvoiceApp README"
purpose: "Project overview"
author: "root_agent"
date: "2025-06-27"
---

# 🎛️ InvoiceApp

[![CI](https://github.com/luckydizzier/wrecept/actions/workflows/ci.yml/badge.svg)](https://github.com/luckydizzier/wrecept/actions/workflows/ci.yml)

**A retro-modern invoice recording desktop application inspired by DOS-era logistics systems.**

---

## 📦 Project Purpose

InvoiceApp originally started as a Windows-only WPF application under the name **Wrecept**. The project has since moved to **.NET MAUI** to enable cross-platform use while keeping the retro look and predictable behavior.

---

## ✨ Features (Planned)

| Feature                          | Status                  |
| -------------------------------- | ----------------------- |
| Retro-style UI                   | ✅ Stage design complete |
| Structured top menu              | ✅ StageView in place    |
| Invoice recording (header/items) | ⏳ UI skeleton ready     |
| Product master data              | ⏳ List view available   |
| Supplier selection               | ⏳ List view available   |
| SQLite-powered persistence       | 🔒 Deferred             |
| Audio-visual feedback            | 🔒 Deferred             |
| Backup & recovery after outage   | 🔒 Deferred             |

---

## 🎹 Interface Philosophy

* **No mouse. No clutter.**
* All screens mimic DOS layouts — with color-coded panels and full-screen efficiency.
* Most of the menus are still placeholders, but product management already works.

## ⌨️ Keyboard Navigation

Keyboard input works the same across Windows, macOS and Linux. A global
`KeyboardNavigator` class converts key presses into commands while keeping the
focus explicit.

| Shortcut | Action |
| -------- | ------------------------------ |
| **F1**   | Focus the sidebar |
| **F2**   | Create a new invoice |
| **F3**   | Edit the selected invoice |
| **Ctrl+F** | Activate the search box |

`Enter` moves to the next cell and saves on the final field. `Esc` always
closes the current dialog or cancels editing.

---

## 📁 Folder Structure

```
InvoiceApp.Core/       # Domain models and service interfaces
InvoiceApp.Data/       # EF Core data access and repositories
InvoiceApp.MAUI/       # MAUI UI project
docs/                  # Documentation
tools/                 # Helper scripts
CHANGELOG.md
InvoiceApp.sln
```

---

## 🛠 Technologies

* Language: **C#** (.NET 8)
* UI: **.NET MAUI**
* Platform: **Windows/Linux/macOS**

---

## 🎯 Next Steps

1. Build out invoice editor UI (inspired by screenshot #3)
2. Integrate fake data into product and supplier modules
3. Discuss minimal database model based on real .dbf structure
4. Mandatory startup tasks can be found in the "Kick OFF" section of [DEV_SPECS.md](DEV_SPECS.md)

## ✅ Kick OFF

The MAUI project lives in `InvoiceApp.MAUI` and contains the following basics:

* `App.xaml` and `App.xaml.cs` – application configuration
* `MainPage.xaml` – main window
* `App.xaml.cs` holds DI and startup logic
* `MainPage` loads the `StageView` layout

These ensure the program runs immediately in a Windows environment.

---

## ✅ Running Tests

Tests can be run with the following command:

```bash
dotnet test tests/InvoiceApp.Core.Tests/InvoiceApp.Core.Tests.csproj
dotnet test tests/InvoiceApp.MAUI.Tests/InvoiceApp.MAUI.Tests.csproj
```

## 📦 Packaging

The MAUI project can be published for multiple platforms. For Windows an MSIX
installer is created with:

```bash
dotnet publish InvoiceApp.MAUI -f net8.0-windows10.0.19041.0 -c Release \
  -p:WindowsPackageType=MSIX
```

macOS and Linux builds use the standard `dotnet publish` command:

```bash
dotnet publish InvoiceApp.MAUI -f net8.0-maccatalyst -c Release
dotnet publish InvoiceApp.MAUI -f net8.0-linux -c Release
```

---

## 🛠️ Migration Notes

The original projects `Wrecept.Core` and `Wrecept.Storage` have been merged into
`InvoiceApp.Core` and `InvoiceApp.Data`. All namespaces now start with
`InvoiceApp`. When updating an existing checkout:

1. Remove the old projects from your solution.
2. Restore dependencies and run the database migrations from `InvoiceApp.Data`.
3. Update any custom code references from `Wrecept.*` to `InvoiceApp.*`.

Application data is now stored under `%AppData%/InvoiceApp` instead of the old
`%AppData%/Wrecept` directory.

---

## 🧾 Credits

Original layout, logic and color schema: \[Egon’s legacy Clipper app]
Reconstruction by: \[ChatGPT-Dev Agent – 2025 Edition]

> "The stage is set, the mixer is ready. Time to wire things up."

---

*Work in Progress – Not intended for production use (yet).*

---

## 📚 Documentation

- [ARCHITECTURE.md](ARCHITECTURE.md) – Layers and data flow
- [AGENTS.md](AGENTS.md) – Agent responsibilities
- [CODE_STANDARDS.md](CODE_STANDARDS.md) – Coding guidelines
- [DEV_SPECS.md](DEV_SPECS.md) – Development specification
- [ERROR_HANDLING.md](ERROR_HANDLING.md) – Fault tolerance
- [FAULT_PLAN.md](FAULT_PLAN.md) – Fault injection plan
- [TEST_STRATEGY.md](TEST_STRATEGY.md) – Test strategy
- [PROJECT_STRUCTURE.md](PROJECT_STRUCTURE.md) – File-level overview
- [MAUI_PORT_PREP.md](MAUI_PORT_PREP.md) – WPF audit and MAUI migration notes
- [../.git-branch-policy.md](../.git-branch-policy.md) – Git branch policy
- [about.md](about.md) – Program information (EN)
- [release_notes_0_0_1.md](release_notes_0_0_1.md) – Initial MVP release notes
- [manuals/developer_guide_hu.md](manuals/developer_guide_hu.md) – Developer guide (HU)
- [manuals/user_manual_hu.md](manuals/user_manual_hu.md) – User manual (HU)
