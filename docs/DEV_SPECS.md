# 📘 Development Specification – Wrecept

---
title: "Development Specification"
purpose: "Technical overview"
author: "root_agent"
date: "2025-06-27"
---

## 🎯 Purpose

Wrecept is an offline-first, single-user application for invoice recording and procurement workflows. After evaluating multi-platform options, we return to Windows-only WPF for simplicity and speed.

The design must be:

* Foolproof for the end user
* Predictable and stable at runtime
* Robust against compile-time failure
* Efficient without being overengineered

---

## 👤 Target User Profile

* Non-technical operator
* Familiar with legacy DOS UI
* Seeks high speed, reliability, and visual confirmation
* Works with a fixed set of suppliers and products

---

## 🔐 System Requirements

| Constraint  | Description                                      |
| ----------- | ------------------------------------------------ |
| OS          | Windows 10+ (x64)                                |
| Network     | No dependency (offline-first)                    |
| Storage     | Local SQLite file with journaling (WAL)          |
| Backups     | Manual + optional autosave-based copy            |
| Permissions | No admin required. Writes to `%AppData%\Wrecept` |

Az alkalmazás minden adatbázis-kapcsolat nyitásakor lefuttatja a `PRAGMA journal_mode=WAL` parancsot, így a naplózási mód mindig visszaáll WAL értékre.

---

## 🧱 Architectural Principles

| Area           | Decision                                             |
| -------------- | ---------------------------------------------------- |
| UI Framework   | WPF (.NET 8)                                  |
| Persistence    | SQLite + Entity Framework Core                       |
| Style          | Retro terminal (green/purple on black), themeable    |
| Input          | Only Enter, Esc, Up, Down allowed for core workflows |
| Code Structure | Agent-based modular design, no tight coupling        |
| ORM Mapping    | One-to-one with simplified domain model              |

---

## 📋 Functional Modules

| Module            | Description                                      |
| ----------------- | ------------------------------------------------ |
| `MainWindow`      | Shell container with top menu and dynamic status bar |
| `InvoiceEditor`   | Header + item rows UI with streamlined control |
| `ProductMaster`   | Searchable product registry and editor           |
| `SupplierMaster`  | Simple list-and-edit view of vendors             |
| `LookupDialog<T>` | Generic search/select component for lookups      |
| `SettingsWindow`  | Audio/visual preferences and toggles             |
| `MaintainingWindow`  | Application/Database maintenance              |
| `HelpWindow`  | Shows the app usage from the documentations          |
| `ExitDialog`  | Possible a question (Wanna quit? Enter/Esc)          |

## 🔍 View Utilities

`VisualTreeExtensions.FindAncestor<T>` segít a vizuális fa bejárásában, ha a
szükséges szülő vezérlőt XAML-ben szeretnénk elérni. Így a nézetekben nem kell
kóddal keresni az ős elemeket, a logika tisztán a ViewModelben marad.


---

## 🧠 Behavioral Constraints

* **No crashes allowed.** Invalid input must be prevented or recoverable.
* **Navigation must not trap the user.** Esc always exits or cancels.
* **Auto-recovery:** if app is restarted after crash, previous working state should be reloaded.
* **Status bar and sounds must mirror current action state.**
* **Menus may display unimplemented features**, but must never trigger an error.

---

## 🔧 Error Handling Requirements

| Scope      | Defense Mechanism                                                  |
| ---------- | ------------------------------------------------------------------ |
| Data entry | Live validation, invalid characters filtered                      |
| Storage    | Pre-check file locks, auto-rollback if insert fails                |
| UI         | Graceful degradation of controls if bindings fail                  |
| Startup    | Verify db existence and schema match, show error window if invalid |
| Input      | Centralized key dispatcher + route safeguards                      |

---

## 📦 Filesystem Layout

```plaintext
%AppData%\Wrecept\
├── app.db               # SQLite database
├── backup\              # Scheduled backups
├── settings.json        # User preferences (theme, sound)
├── logs\                # Runtime error logs (timestamped)
├── Themes\              # Application Themes
└── version.txt          # Last known app version
```
Fejlesztéskor a `wrecept.db` nevű adatbázis kizárólag a migrációk generálásához használatos.
Ha az adatbázis elérési útja hiányzik, a program automatikusan a fenti `%AppData%/Wrecept/app.db` fájlt hozza létre.

---

## 🧾 Known Constraints

* Data import from `.dbf` will be handled later as a one-time tool
* No network sync planned
* Printing/export not required in first milestone
* ORM mapping to match legacy fields where possible, but with normalized naming

---

## ✅ Kick OFF
* Developer and user manuals/documentation in both English and Hungarian
* Base project structure
* Assigned tasks with agents


## ✅ Milestone 1 Deliverables

* Visual-only main menu and layout stub (`StageView`)
* `InvoiceEditor` view with mocked-up item rows
* Basic product and supplier search views
* No database functionality, just layout and input handling

---

*Prepared for: ChatGPT Codex
Maintained by: root\_agent
Date: 2025-06-27*
