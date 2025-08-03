# AGENTS.md

## 🧠 Agent-Based Development Structure for `wrecept`

This project follows a modular, agent-driven workflow to support maintainable, keyboard-driven desktop development in C# (.NET, WPF). Each agent has a clearly defined scope and responsibilities. All prompts, commits, and logs must refer to agent roles.

---

### 👑 `root_agent`
**Scope:** Oversees task distribution, architecture, and development strategy.

- Reads `TODO.md` and assigns tasks to agents.
- Resolves `NEEDS_HUMAN_DECISION` items.
- Manages roadmap and milestone planning.
- Ensures workflow protocol and styleguide compliance.

---

### 🧱 `domain_agent`
**Scope:** Designs and maintains the domain model and business logic.

- Defines entities (Invoice, Customer, Product, TaxRate, etc.)
- Handles validation logic (e.g. invoice total consistency)
- Coordinates with `db_agent` to reflect model updates in schema.
- Ensures domain layer remains framework-agnostic.

---

### 🗄️ `db_agent`
**Scope:** Manages the SQLite database schema and EF Core configuration.

- Creates and maintains `.csproj` EF Core migrations.
- Maps domain entities to database models.
- Applies indexing, constraints, default values.
- Ensures data integrity and optimizes performance.
- Tracks changes in `docs/migrations/` and `README`.

---

### 💡 `logic_agent`
**Scope:** Implements core service logic and orchestration.

- Coordinates between repositories and domain models.
- Implements services (e.g. `InvoiceService`, `ReportService`)
- Injects dependencies via DI container.
- Handles file exports, tax logic, print preview calculation.

---

### 🧪 `test_agent`
**Scope:** Creates and maintains test coverage.

- Uses xUnit or NUnit to test domain logic and services.
- Ensures CI-safe, automated tests for critical workflows.
- Verifies keyboard navigation and input edge cases.
- Reports coverage gaps in `docs/coverage/`.

---

### 🎨 `ui_agent`
**Scope:** Designs the WPF UI layer using MVVM.

- Builds `View`, `ViewModel` pairs with keyboard-only navigation.
- Implements keyboard shortcuts, focus cycles, and accessibility.
- Handles `DataTemplate`, `UserControl`, and `ResourceDictionary` setup.
- Coordinates bindings with `logic_agent`.

---

### 📝 `doc_agent`
**Scope:** Maintains project documentation and logs.

- Updates `README.md`, `docs/progress/*.md`, and `docs/styleguide.md`.
- Tracks agent actions with timestamped logs.
- Updates `TODO.md` task statuses and links.
- Generates API/interface diagrams, workflow maps, etc.

---

### 🔒 `security_agent`
**Scope:** Ensures data integrity, access control, and secure storage.

- Reviews user input handling for injection risks.
- Proposes encryption for local storage, if needed.
- Validates audit logging and potential role-based access control.

---

### 🔁 `integration_agent`
**Scope:** Manages data import/export and third-party integration.

- Handles JSON, CSV, PDF, or XML import/export modules.
- Integrates with tax authority endpoints (e.g. NAV online számla API).
- Builds offline-safe sync logic for future modules.

---

### 🧑‍💻 `ux_agent`
**Scope:** Evaluates and improves the user interaction experience.

- Simulates workflows based on keyboard use.
- Suggests improved layout/flow for data entry.
- Coordinates with `ui_agent` for user feedback loops.

---

## 🛑 Rules
- Agents may collaborate but must respect scope.
- Cross-cutting concerns require coordination and log reference.
- No code or document is valid without task reference (`TODO.md`) and agent signature in logs.

---

