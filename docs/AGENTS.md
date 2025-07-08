# 🤖 AGENTS.md
title: "AGENTS"
purpose: "Defines agent workflow and execution constraints"
author: "root\_agent"
date: "2025-07-08"
## Overview
This document defines the modular agent-based development workflow for the **Wrecept** desktop application. Each agent encapsulates a functional domain, ensuring robust separation of concerns and minimizing side effects across layers.
> **Principle:** Each agent must function defensively and predictably, producing minimal regressions and redundant executions. Inputs, outputs, dependencies, and invariants must be explicit.
## 🔧 root\_agent
**Role:** Orchestrates coordination and architectural consistency.
Inputs:
- docs/README.md, AGENTS.md, ARCHITECTURE.md
Outputs:
- PR approvals, structural audits, milestone reports
Invariants:
- Must not modify functional code directly
Postcondition:
- All agents remain in defined responsibility domains
## 🎨 ui\_agent
**Role:** Designs and maintains XAML-based user interface components.
Inputs:
- ViewModel structures
- Themes (RetroTheme.xaml)
Outputs:
- *.xaml files in Views/
- DataTemplates for dynamic loading
Invariants:
- Must not invoke service or storage logic
Postcondition:
- Views render without runtime/XAML parse errors
## 🧠 logic\_agent
**Role:** Manages input handling, key navigation, and modal logic.
Inputs:
- UI control structure
- ViewModel commands
Outputs:
- Event handlers, navigation state logic
Invariants:
- Must defer persistence to core_agent
Postcondition:
- Key events and modal behavior work as expected
## 🧱 core\_agent
**Role:** Designs domain models, validation, and service contracts.
Inputs:
- ARCHITECTURE.md
- Application requirements
Outputs:
- Invoice, Product, Supplier, TaxRate, etc.
- Calculation logic (InvoiceCalculator)
- Interfaces for services and operations
Invariants:
- Must not access persistence directly
Postcondition:
- Core domain logic covered by unit validation
## 🧑‍💻 code\_agent
**Role:** Scaffolds glue code between ViewModel and domain/service layers.
Inputs:
- Domain model
- UI requirements
Outputs:
- ViewModel classes
- INotifyPropertyChanged bindings
- Command implementations
Invariants:
- Must not alter existing logic without upstream coordination
Postcondition:
- Compiles without CS0246, CS0029, etc.
## 📦 storage\_agent
**Role:** Implements SQLite data persistence via EF Core.
Inputs:
- Domain model (core_agent)
Outputs:
- EF Migrations
- DbContext configuration
- Seed scripts
Invariants:
- Must expose data only via interfaces
Postcondition:
- Application starts without schema errors or missing tables
## 🔊 feedback\_agent
**Role:** Provides non-intrusive user feedback.
Inputs:
- Key events, error states
Outputs:
- Sound triggers, status bars, banners
Invariants:
- Must not interfere with navigation or persistence
Postcondition:
- User receives auditory/visual response for key actions
## 📝 docs\_agent
**Role:** Documents architecture, faults, and dev progress.
Inputs:
- Commit logs, milestone plans
Outputs:
- *.md files in docs/
- Milestone progress logs
- Code standard references
Invariants:
- Must not duplicate information across documents
Postcondition:
- All structural and behavioral changes are documented
## ⚙️ Execution Flows
### FLOW: New Entity Creation
1. `core_agent` – define domain model with validation
2. `storage_agent` – add migration + seed
3. `code_agent` – scaffold ViewModel and services
4. `ui_agent` – create View and DataTemplate
5. `logic_agent` – wire key handling and navigation
6. `test_agent` (if enabled) – validate field behavior
7. `root_agent` – audit and merge
### FLOW: New UI Interaction
1. `ui_agent` – design interaction surface
2. `logic_agent` – handle input logic
3. `feedback_agent` – connect response feedback
4. `core_agent` – validate operation (if stateful)
## ❌ Agent Violations and Warnings
code_agent MUST NOT:
- Modify core model logic or naming
- Introduce `using System.Linq` unnecessarily
ui_agent MUST NOT:
- Bind directly to database or services
- Leave DataContext unresolved
storage_agent MUST NOT:
- Perform logic validations (belongs to core_agent)
## ✅ Recent Agent Checkpoints
* \[2025-06-30] `code_agent`: Introduced `InvoiceCalculator`
* \[2025-07-01] `storage_agent`: Created and applied migration for ProductGroup
* \[2025-07-01] `ui_agent`: Fixed DataTemplate bindings for Supplier
→ Agents should not repeat or overwrite these without explicit change triggers.
*Maintained by `root_agent`. Last updated: 2025-07-01.*
