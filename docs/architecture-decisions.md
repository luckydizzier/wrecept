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

## Domain Boundaries
- `Wrecept.Domain` encapsulates shared entities and value objects.
- `Wrecept.Core` implements services and repositories operating on those types.
- `Wrecept.UI` provides view models and views that consume core services.

## Data Persistence
- Entity Framework Core with SQLite enables offline storage.
- Migrations are versioned under `docs/migrations` to trace schema changes.

## Testing Strategy
- xUnit test projects mirror production modules.
- Core and domain tests run cross-platform; UI tests require Windows.

## Cross-platform Build Strategy
To keep builds portable, only the core libraries and domain tests run on non-Windows hosts. WPF UI projects depend on the WindowsDesktop SDK and are skipped outside Windows environments.

This approach relies on:

- A dedicated solution (`Wrecept.Core.sln`) and solution filter (`wrecept-core.slnf`) that contain only cross-platform projects.
- CI and local scripts invoking `dotnet build Wrecept.Core.sln`, which omits Windows-only projects.
- Conditional project configurations that ignore WPF projects when the `WindowsDesktop` SDK is unavailable.
