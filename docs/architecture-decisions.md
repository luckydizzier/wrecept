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

## Error Handling Strategy
- Services validate inputs and throw domain-specific exceptions.
- A central service captures unhandled exceptions and logs them via Serilog.
- UI components display errors through `IMessageService` for consistency.
- Background tasks return structured error results instead of throwing.

## Cross-platform Build Strategy
To keep builds portable across operating systems, only the core libraries and domain tests run on non-Windows hosts. WPF UI projects depend on the `WindowsDesktop` SDK and are skipped outside Windows environments.

Developers on Linux or macOS should:

- build with `dotnet build Wrecept.Core.sln` or the `wrecept-core.slnf` solution filter to compile cross-platform projects only. Both files contain only cross-platform projects; you may use either for this purpose.
- run tests via `dotnet test Wrecept.Core.Tests` and `dotnet test tests/Wrecept.Domain.Tests`.

Windows developers can compile the entire application with `dotnet build wrecept.sln`.

This approach relies on:

- A dedicated solution (`Wrecept.Core.sln`) and solution filter (`wrecept-core.slnf`) that contain only cross-platform projects.
- CI and local scripts invoking `dotnet build Wrecept.Core.sln` on non-Windows hosts.
- Conditional project configurations that ignore WPF projects when the `WindowsDesktop` SDK is unavailable.
## Localization Strategy
- The primary user interface language is **Hungarian**.
- Text resources are stored in `.resx` files under `Resources/` to enable translation.
- A default resource set provides fallback strings when a translation is missing.
- Date, number, and currency formats follow the current culture at runtime.
- New features must include resource entries for all user-facing text.

## Documentation Strategy
- Progress logs under `docs/progress/` capture timestamped updates for traceability and use the naming convention `YYYY-MM-DD_HH-MM-SS_agentname.md`.
- Each progress log entry references its related task or milestone.
- Architecture decision records are revised alongside progress logs to preserve context.
- Documentation updates are logged to help future contributors understand project history.
