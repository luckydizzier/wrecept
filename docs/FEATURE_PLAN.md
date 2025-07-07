# 📌 FEATURE\_PLAN.md

---

title: "Feature Execution Blueprint"
author: "root\_agent"
date: "2025-07-08"
------------------

This document defines planned features for the **Wrecept** application. Each feature is broken down by required agents, execution order, blocking dependencies, and success criteria.

> Use this file to minimize redundant iterations and maximize agent coordination.

---

## 🆕 Feature: Add Payment Method Entity

**Status:** DONE

### 🎯 Goal

Allow invoices to store a reference to a selectable payment method (e.g., cash, transfer).

### 🧩 Agent Breakdown

| Step | Agent           | Task Description                                     | Blocking Dependency |
| ---- | --------------- | ---------------------------------------------------- | ------------------- |
| 1    | `core_agent`    | Define `PaymentMethod` model with `Id`, `Name`       | -                   |
| 2    | `storage_agent` | Add EF migration, create seed (Cash, Card, Transfer) | core\_agent         |
| 3    | `code_agent`    | Scaffold `PaymentMethodViewModel`, bindable list     | storage\_agent      |
| 4    | `ui_agent`      | Create dropdown in InvoiceEditor                     | code\_agent         |
| 5    | `logic_agent`   | Wire Enter/Esc handling, update selection logic      | ui\_agent           |
| 6    | `root_agent`    | Merge, test regressions                              | all                 |

### ✅ Success Criteria

* User can select a payment method when editing invoice
* Selection is persisted and loaded correctly
* Dropdown is keyboard-navigable and style-compliant

---

## 🧮 Feature: Implement Auto-Totals in InvoiceEditor

**Status:** DONE

### 🎯 Goal

Automatically recalculate invoice totals when line items are added/removed.

### 🧩 Agent Breakdown

| Step | Agent         | Task Description                                     | Blocking Dependency |
| ---- | ------------- | ---------------------------------------------------- | ------------------- |
| 1    | `core_agent`  | Provide `InvoiceCalculator` with net/gross logic     | -                   |
| 2    | `code_agent`  | Inject service into ViewModel, expose summary fields | core\_agent         |
| 3    | `ui_agent`    | Bind total labels to ViewModel properties            | code\_agent         |
| 4    | `logic_agent` | Call calculator on item change, apply debounce       | ui\_agent           |
| 5    | `root_agent`  | Validate performance and layout                      | all                 |

### ✅ Success Criteria

* Net/gross values update in real time
* Performance acceptable on >20 line items
* No hard crash or null references

---


## 🗃️ Feature Status Tracker

| Feature Name                  | Status      | Last Agent Triggered |
| ----------------------------- | ----------- | -------------------- |
| Add Payment Method Entity     | DONE        | test_agent           |
| Implement Auto-Totals         | DONE        | docs_agent           |

---

*2025-07-08:* Last code coverage run reported 100%.*

*Maintained by `root_agent`. Update before major feature execution.*
