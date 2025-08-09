# AGENTS.md

## 🧠 Agent-Based Development Structure for `wrecept`

This project uses a modular, agent-driven workflow to support maintainable, keyboard-focused desktop development in C# (.NET, WPF). Each agent has a clearly defined scope and responsibilities. All prompts, commits, and logs must refer to agent roles.

---

1. 🧠 Hard Skills 
🔧 Software development
Language skills in C#, Java, C++, Python, or Swift - adapted to the target platform.
Desktop UI framework: WPF, WinForms, MAUI, Qt, Electron, etc.
Database: SQLite (standalone file format, offline mode ideal)
ORM usage: Entity Framework Core, Dapper, or own repository pattern
Design patterns: MVVM / MVC / MRS / DDD - ensure clean architecture
📁 Application structure and file management
Local data file storage, encryption (e.g. passwords, settings)
Upgrade strategy (version management, backup, migration)
Export / import formats (PDF, Excel, NAV-XML, CSV)
📦 Publishing and distribution
MSI or EXE installer creation (e.g. Inno Setup, WiX)
Version identification and changelog management
Digital signature, possible code signing
🔍 Testing
Unit tests (xUnit/NUnit/GoogleTest)
Manual regression checklist
Error handling and logging (Serilog, NLog)
2. 🧩 Mental and cognitive skills (Soft Skills)
🧠 S ystems thinking
Understanding interdependencies (data models, business logic, UI processes)
Minimizing dependencies, preparing for scalability
Long-term maintenance and stability considerations
🔍 Precision and fault tolerance
Error analysis and lessons learned
Use of traceable code systems (version control, logging)
🧭 Se lf-management and persistence
Follow development plan (e.g. roadmap, milestones)
Managing setbacks (motivation, avoiding burnout)
Documentation discipline
🛠️ Problem solving
Stack overflow, GitHub, browsing documentation quickly and efficiently
Conscious management of refactoring and technical debt
3. 🔥 Motivational and goal-oriented factors
🎯 Goal: working product for real life
Not an apprenticeship project, but usable software, for real use (e.g. own restaurant, business of a friend, sole proprietorship partners)
💡 Innovation and a desire for control
Own decisions: not others define features, UI, technology
Freedom and creativity: implement individual ideas immediately
💶 Earning potential or cost reduction
Own use = saving on monthly software fees
Potential future sales, even on a licensing basis
❤️ Emotional attachment, challenge
The system is the "child" of the developer: pride in clean code, stable operation
Experience of full competence = flow experience
🧠 Personal profile through an example
An ideal developer:
Ferenc is a custom developer who wants to manage his own restaurant's billing with localized, fast, offline, keyboard-driven software. He works in C# and WPF with SQLite database. His focus is on NAV-compatible PDF printing and an intuitive, clean interface.
He works alone, but is building a plugin-based architecture with an eye to the future and keeps his own development log. He is also building in documentation, an export module and backup functionality to allow others to try out the software.
Motivation: self-reliance, learning, system building and financial management.

---

### 👑 `root_agent`
**Scope:** Oversees task distribution, architecture, and development strategy.

- Reads `TODO.md` and assigns tasks to agents.
- Resolves `NEEDS_HUMAN_DECISION` items.
- Manages roadmap and milestone planning.
- Ensures workflow protocol and styleguide compliance.
- Lists all open tasks in `TODO.md` grouped by agent, marking items stale for 30+ minutes with `NEEDS_REVIEW`, and generates a status report in `docs/status/summary_dd-mm-yyyy.md`.

---

### 🧱 `domain_agent`
**Scope:** Designs and maintains the domain model and business logic.

- Defines entities (Invoice, Customer, Product, TaxRate, etc.).
- Validates business rules (e.g., invoice VAT-total consistency).
- Coordinates with `db_agent` to reflect model updates in the schema.
- Keeps the domain layer framework-agnostic.

---

### 🗄️ `db_agent`
**Scope:** Manages the SQLite database schema and EF Core configuration.

- Creates and maintains EF Core migrations in the `.csproj`.
- Maps domain entities to database models.
- Applies indexing, constraints, and default values.
- Ensures data integrity and optimizes performance.
- Tracks schema changes in `docs/migrations/` and updates `README`.

---

### 💡 `logic_agent`
**Scope:** Implements core service logic and orchestration.

- Mediates between repositories and domain models.
- Implements services (e.g., `InvoiceService`, `ReportService`).
- Injects dependencies via the DI container.
- Handles file exports, tax logic, and print preview calculations.

---

### 🧪 `test_agent`
**Scope:** Creates and maintains test coverage.

- **Core-logic (CI-safe):**
- xUnit / NUnit tests for Domain, Application and Infrastructure layers. 
- Run in Linux-based CI on `Wrecept.Core.sln` solver file.
- Command: `dotnet test --filter "Category!=UI"`.

- **UI tests (WPF specific):**
- Separate `Wrecept.UI.Tests` project, each test with `[Trait("Category", "UI")]` attribute. 
- `[SkippableFact]` + automatic **Skip** in non-Windows environment.
- Can be fully executed in optional Windows runner; otherwise manual check.

- **Manual feedback handling:**
- If a UI test is run with Skip, *test_agent* will register the task in `docs/user_tests.md`: 
"Awaiting manual feedback - <testname>".

- **Coverage:**
- Generates coverage report (`docs/coverage/Latest.md`) for core projects after each successful CI run.
- Reports missing areas and updates the badge in `README` if necessary.

---


### 🎨 `ui_agent`
**Scope:** Designs the WPF UI layer using MVVM.

- Builds `View` and `ViewModel` pairs with keyboard-only navigation.
- Implements keyboard shortcuts, focus cycles, and accessibility.
- Manages `DataTemplate`, `UserControl`, and `ResourceDictionary` setup.
- Coordinates bindings with `logic_agent`.

---

### 📝 `doc_agent`
**Scope:** Maintains project documentation and logs.

- Updates `README.md`, `docs/progress/*.md`, and `docs/styleguide.md`.
- Tracks agent actions with timestamped logs.
- Updates `TODO.md` task statuses and links.
- Generates API/interface diagrams and workflow maps.

---

### 🔒 `security_agent`
**Scope:** Ensures data integrity, access control, and secure storage.

- Reviews user input handling for injection risks.
- Proposes encryption for local storage when needed.
- Validates audit logging and potential role-based access control.

---

### 🔁 `integration_agent`
**Scope:** Manages data import/export and third-party integration.

- Implements JSON, CSV, PDF, or XML import/export modules.
- Coordinates external data flows with `security_agent`.

---

### 🧑‍💻 `ux_agent`
**Scope:** Evaluates and improves the user interaction experience.

- Simulates workflows based on keyboard use.
- Suggests layout and flow improvements for data entry.
- Works with `ui_agent` to process user feedback.

---

## 🛑 Rules
- Agents may collaborate but must respect scope boundaries.
- Cross-cutting concerns require coordination and log references.
- No code or document is valid without task reference (`TODO.md`) and agent signature in logs.

---

### Keyboard Map
Ins Add • Del Remove • Enter Save • Esc Cancel • Typing Search

### Acceptance Checklist
- [ ] Inline item editing with keyboard navigation
- [ ] Totals update automatically
- [ ] Invalid fields prevent saving
- [ ] Product lookup fills item data
- [ ] Status bar shows hotkeys and errors

