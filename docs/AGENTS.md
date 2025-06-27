# 🤖 AGENTS.md

---
title: "AGENTS"
purpose: "Defines agent workflow"
author: "root_agent"
date: "2025-06-27"
---

## Overview

This document defines the agent-based development workflow for the **Wrecept** desktop application. Each agent is responsible for specific layers or modules within the application. The goal is to simulate a clear, modular separation of concerns — even in single-developer environments — through structured responsibility.

> **Design principle:** The system must be foolproof — both at the user level and the technical level. Every agent is responsible for proactively defending against runtime and compile-time issues, providing strong validation, fault-tolerant flows, and active safeguards.

---

## 🔧 root\_agent

**Role:** Oversees overall coordination and integrity of the application.

* Loads `docs/README.md` and `docs/AGENTS.md` on startup.
* Ensures all sub-agents respect ownership boundaries.
* Approves design and navigation conventions.
* Coordinates release milestones and feature branching.
* Reviews commits for structural or architectural consistency.
* Ensures all critical paths are covered with fail-safe protections.

---

## 🎨 ui\_agent

**Role:** Manages the layout and behavior of XAML UI elements.

* Builds views under `Views/`, including `MainWindow`, `StageView`, `StatusBar`.
* Enforces keyboard-only navigation (`Enter`, `Esc`, `Up`, `Down`).
* Applies retro-styled colors via `Themes/RetroTheme.xaml`.
* Provides visual safeguards (disabled states, error highlights).
* Never accesses application logic or persistence code.

---

## 🧠 logic\_agent

**Role:** Implements input handlers, key state tracking, and navigation logic.

* Attaches `KeyDown`/`PreviewKeyDown` events to focused controls.
* Coordinates focus movement, field validation, and modal dialogs.
* Delegates saving and lookup triggers to `core_agent`.
* Enforces safe default behaviors, guards against invalid transitions.
* Collaborates with `ui_agent` to ensure consistent UX.

---

## 🧱 core\_agent

**Role:** Designs domain models, service contracts, and shared logic.

* Defines classes like `Invoice`, `Product`, `Supplier`, etc.
* Maintains internal calculation logic (e.g., tax totals).
* Prepares service interfaces for data access and UI logic.
* Validates all operations before passing to persistence.
* Coordinates with `storage_agent` and `logic_agent`.
* `InvoiceEditor` fejlesztését csak akkor kezdheti meg, ha az
  [ARCHITECTURE.md](ARCHITECTURE.md) alapján megértette az adatútvonalakat.

---

## 🧑‍💻 code\_agent

**Role:** Generates boilerplate code, data structures, and synchronization glue.

* Scaffolds `ViewModel` classes, property change logic.
* Generates bindable properties and command handlers.
* Ensures consistent naming, namespace structure, and file layout.
* Adds compile-time guards and defensive programming constructs.
* Cooperates with `core_agent` and `logic_agent`.

---

## 📦 storage\_agent

**Role:** Manages data persistence and schema integration.

* Implements repository pattern using SQLite.
* Uses **Entity Framework Core** as the primary ORM.
* Manages migration scripts, journaling, and data integrity.
* Exposes repository interfaces to `core_agent` only.
* Implements transactional safeguards, fallback recovery.
* Can import legacy `.dbf` files into normalized schema.

---

## 🔊 feedback\_agent

**Role:** Provides audio/visual feedback to the user.

* Plays sounds for keypresses, errors, and confirmations.
* Triggers message banners and status hints.
* Works closely with `ui_agent` and `logic_agent`.
* Provides user-level confidence cues and non-intrusive alerts.
* May be toggled on/off in settings later.

---

## 📝 docs_agent

**Role:** Maintains documentation and progress logs.

* Owns `ARCHITECTURE.md`, `ERROR_HANDLING.md`, `TEST_STRATEGY.md`, `FAULT_INJECTION.md`, and `CODE_STANDARDS.md`.
* Rögzíti a feladatok előrehaladását a `docs/progress/` könyvtárban.
* Egyeztet a `root_agent`-tel, ha a dokumentáció a többi réteg felépítését is érinti.

---

## ✨ Guidelines

* **DO NOT** let agents cross layers without coordination.
* **ALWAYS** reference `docs/README.md` before starting work.
* **USE** placeholder components during early-stage prototyping.
* **TAG** commits with `[agent:name]` for traceability.
* **MINIMIZE** hard-coding; use centralized config and localization.
* **FOLLOW** naming conventions: `Wrecept.Core.Models`, `Wrecept.Desktop.Views`, etc.
* **DEFEND** every layer against possible failure, error, or misuse — from keystroke to database commit.
* Progress logs go to `docs/progress/[UTC-TIME]-[agent:name].md`

---

*Maintained by `root_agent`. Updated: 2025-06-27.*
