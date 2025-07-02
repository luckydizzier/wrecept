# ⌨️ KeyboardFlow.md

---
**title:** Keyboard Navigation Flow
**purpose:** Formal description of keyboard behavior in Wrecept
**author:** logic_agent
**date:** 2025-07-02
---

## 🧭 Navigation Principles

A Wrecept minden felületén a billentyűzet az elsődleges vezérlő eszköz. A `NavigationHelper.Handle` segít az általános, fokozatmentes fókuszmozgatásban, míg az egyes nézetek saját `KeyDown` kezelőkkel finomítják a viselkedést.

## 🔑 Key Bindings Overview

| Key      | Globális hatás                     | Lokális környezet                        | Megjegyzés |
|---------|-----------------------------------|------------------------------------------|------------|
| `Enter` | Fókusz a következő vezérlőre       | Sor megerősítés, inline létrehozó indítása |            |
| `Down`  | Fókusz a következő vezérlőre       | Lista vagy rács navigációja              | „Enter” tükre |
| `Up`    | Fókusz az előző vezérlőre          | Előző szerkeszthető mezőre lép            |            |
| `Escape`| Fókusz vissza a főablakra          | Prompt vagy modal bezárása               |            |
| `Delete`| –                                 | Mesterlistákban tétel törlése            | `BaseMasterView` köti |
| `Tab`   | Alapértelmezett tabuláció         | Csak modális vagy inline szerkesztőben módosított | |

## 🧾 View-Specific Flow

### InvoiceEditorView
- A fejmezőkben `Enter` vagy `Down` továbbítja a fókuszt.
- `Escape` visszaviszi a főablak listájához.
- Az „Inline Item Entry” sor a `OnEntryKeyDown` eseményt használja:
  - `Enter` az utolsó mezőben (`EntryTax`) meghívja az `AddLineItemCommand`-et.
  - Egyébként a `NavigationHelper` lép közbe.
- Az `InvoiceItemsGrid`-en `Enter` az aktuális tétel szerkesztését indítja.

### BaseMasterView
- `Enter`: kijelölt sor szerkesztése.
- `Delete`: kijelölt sor törlése.
- `Escape`: részletes nézetből vissza a listához.
Az összes mesteradat ViewModel az `EditableMasterDataViewModel` leszármazottja, így ezek a billentyűk minden listában azonos módon viselkednek.

## 📦 Modal Prompt Behavior

Az `ArchivePromptView`, `SaveLinePromptView` és `InvoiceCreatePromptView` egyaránt követi:
- `Enter` a megerősítő parancsot futtatja.
- `Escape` a mégse parancsot hívja.
A fókusz a prompt bezárása után visszatér a megnyitó nézethez.

## 📋 Focus Reset Rules

Globálisan az `Escape` mindig a főablakra helyezi a fókuszt:
```csharp
Application.Current.MainWindow?.Focus();
```
Az `Enter` alapértelmezésben a következő vezérlőre ugrik, ha az aktuális kezelő nem nyeli el.

## 💡 Design Philosophy

A billentyűzetes navigációt a sebesség és az időtálló megszokhatóság jegyében terveztük. Minden akció egzaktul megismételhető, vizuális visszajelzéssel kombinálva.

## 🔧 Future Enhancements

- [ ] AccessKey-k hozzárendelése a címkékhez
- [ ] Testreszabható billentyűzetprofil `wrecept.json`-on keresztül
- [ ] `Ctrl+Z` visszavonás a sor szerkesztésben
- [ ] Tesztesetek bővítése a `TEST_MATRIX.md`-ben

---

> “Keyboards don't lie. If the app isn't predictable, the flow isn't finished.” — *root_agent*
