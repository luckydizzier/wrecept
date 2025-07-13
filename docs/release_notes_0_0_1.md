---
title: "Release Notes v0.0.1"
purpose: "Overview of the initial MVP release"
author: "docs_agent"
date: "2025-07-08"
---

# 📝 Release Notes – Version 0.0.1

Wrecept 0.0.1 delivers the first minimal viable product for offline invoice recording. The focus is stability and keyboard-driven workflow for sole proprietors.

## ✨ Functional Highlights

| Code | Function | Details |
| ---- | -------- | ------- |
| F-01 | Invoice entry | Manual invoice number input. For returning suppliers the latest number is offered as a base. |
| F-02 | Invoice list | Shows date, supplier and total. Navigation is keyboard friendly; pressing Enter opens details. |
| F-03 | Filtering | Search by supplier and date range. |
| F-04 | Master data CRUD | Simple lists with editor dialogs for all entities. |
| F-05 | Net/Gross calculation | Totals per tax rate; packaging and returns handled with negative quantities. |
| F-06 | Database self-heal | Missing files are created, schema and indices verified, damaged records logged and rebuilt from last valid state. |
| F-07 | Optional sample data | Offered on first start to seed example content. |

## 🔧 Non-Functional Requirements

- Offline operation without internet access.
- Full keyboard navigation with shortcuts for all actions.
- Robust startup that survives missing or corrupt databases.
- Clear code base using MVVM with Model–Repository–Service separation.
- Internal logging via Serilog in rolling JSON files (5 × 5 MB).

## 🏗️ Architecture Overview

```
Views (MAUI/XAML) ──> ViewModels ──> Services ──> Repositories ──> Models
```

* The View layer contains only UI definitions.
* ViewModels expose state and commands to the View.
* Services implement business rules and calculations.
* Repositories handle SQLite persistence via Dapper.
* Models represent plain data objects.

A `StartupOrchestrator` verifies the database, loads configuration and seeds sample data when requested.

## 🔒 Data Safety at Startup

The startup process checks for missing or corrupt database files. Damaged files are backed up with a timestamp, then a clean schema is generated. Records can be rebuilt using the `change_log` table to restore the last valid state.

---
