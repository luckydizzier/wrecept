# TODO – Wrecept development schedule

## 🟢 LEGEND
- `TODO`: not started yet
- `IN_PROGRESS`: currently in progress
- `DONE`: completed
- `NEEDS_REVIEW`: not updated for 30+ mins
- `NEEDS_HUMAN_DECISION`: requires decision

---

## root_agent
- [x] `DONE` Create and finalise structured `AGENTS.md`
- [x] `DONE` Final build script (standalone, Windows .exe)
- [x] `DONE` Licensing decision (`LICENSE`)

## doc_agent
- [ ] `IN_PROGRESS NEEDS_REVIEW` Validate `styleguide.md` and `UI_FLOW.md`, fill in missing points
- [x] `DONE` First use of the `docs/progress/` logging system
- [x] `DONE` Mention progress logs in `README.md`
- [x] `DONE` Write installation guide (`INSTALL.md`)
- [ ] `TODO NEEDS_REVIEW` Update README.md (feature list, screenshots)

## logic_agent
- [ ] `TODO NEEDS_REVIEW` Set up MRS structure (Model → Repository → Service)
- [ ] `TODO NEEDS_REVIEW` Data recording and saving in a working prototype
- [ ] `TODO NEEDS_REVIEW` Introduction and configuration of Serilog
- [ ] `TODO NEEDS_REVIEW` Load/save application settings to file (`wrecept.json`)

## db_agent
- [ ] `TODO NEEDS_REVIEW` Initialise SQLite database with migration
- [ ] `TODO NEEDS_REVIEW` EF Core configuration (relationships, validation)

## domain_agent
*(no open tasks)*

## ui_agent
- [ ] `TODO NEEDS_REVIEW` Basic `MainView` + `InvoiceEditorView` XAML layout
- [ ] `TODO NEEDS_REVIEW` Implement keyboard-only navigation (Enter, Escape, arrows, Tab)
- [ ] `TODO NEEDS_REVIEW` Introduction of themes (dark/light + customisation)
- [ ] `TODO NEEDS_REVIEW` ThemeEditorView – theme management UI editor
- [ ] `TODO NEEDS_REVIEW` Confirmation popups for all critical actions

## test_agent
- [ ] `TODO NEEDS_REVIEW` Unit tests for the Service layer
- [ ] `TODO NEEDS_REVIEW` Database validations (error handling + rollback)
- [ ] `TODO NEEDS_REVIEW` Testing keyboard navigation logic on different views

## integration_agent
- [ ] `TODO NEEDS_REVIEW` Database export/import function

## ux_agent
- [ ] `TODO NEEDS_REVIEW` Handle audio feedback
- [ ] `TODO NEEDS_REVIEW` Error messages should be displayed in Hungarian
