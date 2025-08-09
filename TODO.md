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
- [x] `DONE` Separate WPF UI into `Wrecept.UI` and create `Wrecept.Core.sln`

## doc_agent
- [x] `DONE` Validate `styleguide.md` and `UI_FLOW.md`, fill in missing points
- [x] `DONE` First use of the `docs/progress/` logging system
- [x] `DONE` Mention progress logs in `README.md`
- [x] `DONE` Write installation guide (`INSTALL.md`)
- [x] `DONE` Update README.md (feature list, menu structure, etc...)
- [x] `DONE` Expand README with detailed feature list and menu structure
- [ ] `TODO` Create keyboard shortcut cheat sheet

## logic_agent
- [x] `DONE` Set up MRS structure (Model → Repository → Service)
- [x] `DONE` Data recording and saving in a working prototype
- [x] `DONE` Introduction and configuration of Serilog
- [x] `DONE` Load/save application settings to file (`wrecept.json`)
- [x] `DONE` Auto-suggestions for recurring invoice items or frequently purchased products
- [x] `DONE` Predictive text entry using local history
- [x] `DONE` Reporting analytics engine for sales trends, top customers/products, and tax breakdowns

## db_agent
- [x] `DONE` Initialise SQLite database with migration
- [x] `DONE` EF Core configuration (relationships, validation)

## domain_agent
- [x] `DONE` Initial core domain entities with validations

## ui_agent
 - [ ] `IN_PROGRESS` Basic `MainView` + `InvoiceEditorView` XAML layout
 - [ ] `IN_PROGRESS` Implement keyboard-only navigation (Enter, Escape, arrows, Tab)
 - [ ] `IN_PROGRESS` Introduction of themes (dark/light + customisation)
 - [ ] `IN_PROGRESS` ThemeEditorView – theme management UI editor
- [x] `DONE` Confirmation popups for all critical actions
- [ ] `IN_PROGRESS` Implement contextual tooltips across the UI
- [ ] `TODO` Provide quick keyboard cheat sheet within the application
- [ ] `IN_PROGRESS` Display auto-suggestions and predictive text in `InvoiceEditorView`
 - [ ] `IN_PROGRESS` Develop customizable dashboard views for financial and inventory metrics

## test_agent
- [ ] `TODO NEEDS_REVIEW` Unit tests for the Service layer
- [ ] `TODO NEEDS_REVIEW` Database validations (error handling + rollback)
- [ ] `TODO NEEDS_REVIEW` Testing keyboard navigation logic on different views
- [x] `DONE` Setup `Wrecept.UI.Tests` with UI category and OS skip

## integration_agent
- [ ] `TODO NEEDS_REVIEW` Database export/import function

## ux_agent
- [x] `DONE` Invoice form grid layout and keyboard workflow analysis
- [ ] `TODO NEEDS_REVIEW` Handle audio feedback
- [ ] `TODO NEEDS_REVIEW` Error messages should be displayed in Hungarian
- [ ] `TODO` Evaluate onboarding flow for first-time users
- [ ] `TODO` Conduct user testing for invoice creation workflow with keyboard-only navigation
- [ ] `TODO` Draft accessible color contrast and font guidelines
- [ ] `TODO` Design user onboarding tour
