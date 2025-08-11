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

This is achieved by using solution filters (`.slnf` files) that exclude WPF UI projects when building on non-Windows platforms. Developers should use the provided solution filter (`wrecept-core.slnf`) for cross-platform builds, which ensures only compatible projects are included. Alternatively, build configuration conditions in the project files prevent WPF UI projects from building on unsupported platforms.
