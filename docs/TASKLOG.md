# 📋 TASKLOG.md – Task Ledger for Wrecept

This file defines the official task structure for the Wrecept project. Tasks are assigned to agents in alignment with `docs/AGENTS.md` and tracked across milestones. Each entry may include a status, responsible agent, and notes.

---

## ✅ Kick OFF

* Create developer and user manuals/documentation in both **English** and **Hungarian**
* Establish base project structure (`docs/`, `Views/`, `Themes/`, etc.)
* Assign functional responsibilities to agents per `AGENTS.md`
* Initialize README, AGENTS.md, DEV\_SPECS.md, and TASKLOG.md
* Confirm agent-based execution with Codex-compatible prompts

---

## 🔄 Task Status Legend

* `[ ]` – Open (not yet started)
* `[~]` – In Progress
* `[x]` – Done
* `[!]` – Blocked or requires human decision

---

## ✅ Milestone 1 Deliverables

* Visual-only main menu and layout stub (`StageView`) — *ui\_agent*
* `InvoiceEditor` view with mocked-up item rows — *ui\_agent + logic\_agent*
* Basic product and supplier search views — *ui\_agent + core\_agent*
* No database functionality, just layout and input handling — *all*
* Full keyboard routing and error suppression for UI prototype — *logic\_agent + feedback\_agent*

---

## 🧠 Notes for Codex

This file serves as both a roadmap and an executable instruction set.

* Agents must **read and act** on their assigned entries.
* Codex may generate scaffolding, stubs, boilerplate, or placeholder files.


> “Don’t just read it. Run it.” – root\_agent

---

*Maintained by: root\_agent
Prepared for: ChatGPT Codex
Date: 2025-06-27*
