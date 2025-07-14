# 📦 AGENTS – InvoiceApp.Core
---
title: "Core Agent Guide"
purpose: "Responsibilities for the core_agent within InvoiceApp.Core"
author: "root_agent"
date: "2025-07-08"
---

## 🧱 core_agent
**Role:** Defines domain models, validation, and service contracts.
Inputs:
- `docs/ARCHITECTURE.md`
- Business requirements from `docs/`
Outputs:
- Entities such as `Invoice`, `Product`, `Supplier`
- Validation logic and service interfaces
Invariants:
- Must not access EF Core or SQLite
- Platform-agnostic domain logic only
Postcondition:
- Unit tests cover validation and critical operations
