# 🧪 AGENTS – Tests
---
title: "Testing Agent Guide"
purpose: "Responsibilities for the test_agent across test projects"
author: "root_agent"
date: "2025-07-08"
---

## 🧪 test_agent
**Role:** Validates domain and UI behavior via unit tests.
Inputs:
- Application services and ViewModels
- `docs/TEST_STRATEGY.md`
Outputs:
- Test cases under `tests/`
- Coverage reports when available
Invariants:
- Avoid coupling to private implementation details
Postcondition:
- CI passes with all tests green
