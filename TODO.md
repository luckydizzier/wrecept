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
 - [x] `DONE` Create keyboard shortcut cheat sheet
 - [x] `DONE` Update README to reflect implemented features
- [x] `DONE` Document AsyncRelayCommand usage in style guide
- [x] `DONE` Document case-insensitive product search in README

## logic_agent
- [x] `DONE` Set up MRS structure (Model → Repository → Service)
- [x] `DONE` Data recording and saving in a working prototype
- [x] `DONE` Introduction and configuration of Serilog
- [x] `DONE` Load/save application settings to file (`wrecept.json`)
- [x] `DONE` Auto-suggestions for recurring invoice items or frequently purchased products
- [x] `DONE` Predictive text entry using local history
- [x] `DONE` Reporting analytics engine for sales trends, top customers/products, and tax breakdowns
- [x] `DONE` Handle corrupted settings file gracefully
- [x] `DONE` Make product lookup case-insensitive using EF.Functions.Like

## db_agent
- [x] `DONE` Initialise SQLite database with migration
- [x] `DONE` EF Core configuration (relationships, validation)
- [x] `DONE` Add NOCASE collation and indexes for product and supplier names

## domain_agent
- [x] `DONE` Initial core domain entities with validations

## ui_agent
- [x] `DONE` Basic `MainView` + `InvoiceEditorView` XAML layout
- [x] `DONE` Implement keyboard-only navigation (Enter, Escape, arrows,Insert, Delete (no Tab))
- [x] `DONE` Introduction of themes (dark/light + customisation)
- [x] `DONE` ThemeEditorView – theme management UI editor
- [x] `DONE` Confirmation popups for all critical actions
- [x] `DONE` Implement contextual tooltips across the UI
- [ ] `TODO` Provide quick keyboard cheat sheet within the application
- [x] `DONE` Display auto-suggestions and predictive text in `InvoiceEditorView`
- [ ] `IN_PROGRESS` Develop customizable dashboard views for financial and inventory metrics
- [x] `DONE` Replace blocking call in `ThemeEditorViewModel` with async initialization
- [x] `DONE` Replace async void commands with `AsyncRelayCommand`

## test_agent
- [x] `DONE` Unit tests for the Service layer
- [x] `DONE` Database validations (error handling + rollback)
- [ ] `TODO NEEDS_REVIEW` Testing keyboard navigation logic on different views
- [x] `DONE` Setup `Wrecept.UI.Tests` with UI category and OS skip
- [x] `DONE` Add unit test for settings theme update
- [x] `DONE` Add unit test for `ThemeEditorViewModel` initialization
- [x] `DONE` Add unit test for case-insensitive product lookup

## integration_agent
- [x] `DONE` Database export/import function

## ux_agent
- [x] `DONE` Invoice form grid layout and keyboard workflow analysis
- [ ] `TODO NEEDS_REVIEW` Handle audio feedback
- [ ] `TODO NEEDS_REVIEW` Error messages should be displayed in Hungarian
- [ ] `TODO` Evaluate onboarding flow for first-time users
- [ ] `TODO` Conduct user testing for invoice creation workflow with keyboard-only navigation
- [ ] `TODO` Draft accessible color contrast and font guidelines
- [ ] `TODO` Design user onboarding tour
