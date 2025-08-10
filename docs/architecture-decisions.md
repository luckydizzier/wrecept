# Architecture Decisions

This document records key architectural choices for the **wrecept** application.

## Overall Principles
- Offline-first design with local SQLite storage.
- Model–Repository–Service (MRS) layering to separate concerns.
- MVVM pattern for WPF user interface.
- Dependency Injection configured through a central startup orchestrator.
- Structured logging via Serilog.

## Rationale
These decisions aim to keep the codebase modular, testable, and maintainable while supporting the project's keyboard-centric workflow.
