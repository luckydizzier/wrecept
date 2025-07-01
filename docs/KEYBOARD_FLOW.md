# ⌨️ KeyboardFlow.md

---
**title:** Keyboard Navigation Flow  
**purpose:** Defines standard keyboard interactions across Wrecept views  
**author:** root_agent  
**date:** 2025-07-02  
---

## 🧭 Navigation Principles

The Wrecept application follows a **keyboard-first interaction model**, inspired by retro terminal systems but extended for modern UX needs.

All views support intelligent focus navigation, using `NavigationHelper.Handle(KeyEventArgs)` and local handlers (e.g. `OnEntryKeyDown`) for custom logic.

---

## 🔑 Key Bindings Overview

| Key            | Global Scope          | Local Scope (Context-sensitive)                         | Notes |
|----------------|-----------------------|----------------------------------------------------------|-------|
| `Enter`        | Move focus to next    | Confirm field / Start inline creator / Add new item     |       |
| `Down`         | Move focus to next    | Navigate dropdowns or DataGrid                          | Mirrors Enter |
| `Up`           | Move focus to previous| Navigate to prior editable field                        |       |
| `Escape`       | Reset focus to main window | Cancel prompt / Close modal / Blur active cell    |       |
| `Tab`          | Default tab behavior  | *(overridden only in modal or inline editors)*          |       |
| `Ctrl+S`       | _(deprecated)_        | Save — to be replaced with autosave / SaveCommand       | Marked for removal |
| `F2`           | _(deprecated)_        | Manual Edit — legacy behavior                           |      |
| `Ctrl+L`       | _(deprecated)_        | Lookup / Search — replaced by inline EditLookup         |      |

---

## 🧾 InvoiceEditorView Specific Flow

### Header Fields Navigation

- Pressing `Enter`/`Down` in each field advances to the next logical field.
- `Escape` exits the field and resets focus to window level.

### Inline Item Entry Row

| Field          | Behavior on `Enter`      | Behavior on `Escape`        |
|----------------|--------------------------|-----------------------------|
| Product (EditLookup) | Triggers inline creator if no match | Blurs input |
| Quantity (TextBox)   | Advances to Unit    |                             |
| Unit (EditLookup)    | Advances to Price   |                             |
| Price (TextBox)      | Advances to Tax     |                             |
| Tax (EditLookup)     | Commits AddItem()   |                             |

- Entire row listens on `KeyDown` via `OnEntryKeyDown`.
- `Enter` in the last field (Tax) finalizes the line.

---

## 📦 Archive Prompt & Save Prompt

- `Escape`: Closes prompt immediately.
- `Enter`: Confirms if the prompt is focused.

> All prompts use `ContentControl` and boolean visibility binding.
> Keyboard handling relies on the viewmodel’s state and standard navigation.

---

## 📋 Focus Reset Behavior

- If the user presses `Escape` from any nested control:
  - `Application.Current.MainWindow.Focus()` is called.
  - This allows the `StageView` ContentControl to regain interaction.

---

## 💡 Design Philosophy

- Focus always moves **predictably and directionally**.
- Navigation is **linear**, avoids modal traps.
- `EditLookup` acts as a dual-purpose selector + inline-creator.
- Keyboard flow is optimized for **numeric keypad + single-hand use**.

---

## 🔧 Future Enhancements

- [ ] Define `AccessKey` accelerators for form labels (Alt+…)
- [ ] Allow custom keyboard profile via `wrecept.json`
- [ ] Support `Ctrl+Z` undo within line items
- [ ] Define test cases in `TEST_MATRIX.md`

---

> “The keyboard is the scalpel. The screen is the field. Let the workflow be surgical.”
> — *root_agent*