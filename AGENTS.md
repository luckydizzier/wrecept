# AGENTS.md

## 🧠 Agent-Based Development Structure for `wrecept`

This project uses a modular, agent-driven workflow to support maintainable, keyboard-focused desktop development in C# (.NET, WPF). Each agent has a clearly defined scope and responsibilities. All prompts, commits, and logs must refer to agent roles.

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

- Uses xUnit or NUnit to test domain logic and services.
- Ensures CI-safe, automated tests for critical workflows.
- Verifies keyboard navigation and input edge cases.
- Reports coverage gaps in `docs/coverage/`.

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

