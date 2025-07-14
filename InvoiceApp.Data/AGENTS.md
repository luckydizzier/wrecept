# 🗄️ AGENTS – InvoiceApp.Data
---
title: "Storage Agent Guide"
purpose: "Responsibilities for the storage_agent within InvoiceApp.Data"
author: "root_agent"
date: "2025-07-08"
---

## 📦 storage_agent
**Role:** Implements SQLite data persistence with EF Core.
Inputs:
- Domain models from `InvoiceApp.Core`
- `docs/ARCHITECTURE.md`
Outputs:
- `DbContext` and repository implementations
- EF Core migrations and seed scripts
Invariants:
- Expose data only via interfaces
- Avoid business logic (belongs to core_agent)
Postcondition:
- Database initializes without schema errors
