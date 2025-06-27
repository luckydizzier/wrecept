# 📋 TASKLOG.md – Task Ledger for Wrecept

Minden fejlesztési feladat itt kerül nyilvántartásra, szinkronban a `ROADMAP.md` és az `AGENTS.md` dokumentumokkal. Minden tételhez tartozik egy milestone hivatkozás, felelős ügynök, státusz, valamint opcionálisan egy megjegyzés vagy `NEEDS_HUMAN_DECISION` címke.

---

## 🔄 Állapotjelölések

* `[ ]` – **Open** (még nem elkezdett)
* `[~]` – **In Progress** (folyamatban)
* `[x]` – **Done** (befejezett, naplózott)
* `[!]` – **Blocked** vagy **Human Decision szükséges**

---

## 📌 M1 – Project Setup

* [x] `docs/`, `src/`, `tests/` könyvtárszerkezet létrehozása — *Architect*
* [x] MIT License file hozzáadása — *Architect*
* [x] Alap README létrehozása — *DocWriter*
* [x] `.gitignore`, `.editorconfig` inicializálása — *Architect*

---

## 🧠 M2 – Cross-Platform Core

* [x] `Wrecept.Core` projekt létrehozása — *CodeGen-CSharp*
* [x] Entitásosztályok és interfészek definiálása — *CodeGen-CSharp*
* [x] xUnit tesztprojekt inicializálása — *TestWriter*
* [x] `Specs/CoreServices.md` specifikáció — *Architect*
* [x] `docs/architecture.md` bővítése a core API-val — *DocWriter*

---

## 🧪 M3 – WPF UI Prototype

* [x] MVVM keretrendszer bevezetése — *CodeGen-CSharp*
* [x] `InvoiceListView.xaml` létrehozása — *CodeGen-XAML*
* [x] Tab, PgUp, PgDn működés — *ux\_agent*
* [x] Alapértelmezett fókuszmező és layout — *CodeGen-XAML*
* [x] Alkalmazás indulási nézete — *Architect*

---

## ⚙ M3.1 – Persistence, Filters and Settings

* [x] SQLite repository implementáció — *CodeGen-CSharp*
* [x] Dátum- és szállító-szűrő dialógusok — *CodeGen-XAML*
* [x] `SettingsView.xaml` + JSON konfig — *CodeGen-XAML*
* [x] Hiányzó adatbázis automatikus újragenerálása — *CodeGen-CSharp*
* [x] DB lock és corrupt recovery — *CodeGen-CSharp*

---

## 🔌 M4 – Plugin Framework

* [x] Plugin loader és interfész implementálása — *CodeGen-CSharp*
* [x] Plugin API dokumentáció — *DocWriter*
* [x] ExtensionPoint tervezés — *Architect*

---

## 🚀 M5 – Release Preparation

* [x] `setup.sh` script — *Architect*
* [x] Inno Setup szkript — *Architect*
* [x] InlineCreator komponens létrehozása — *CodeGen-CSharp*
* [x] Export és nyomtatás funkciók — *CodeGen-CSharp + CodeGen-XAML*

---

## 🧹 M6 – Post Release Cleanup

* [x] InlineCreator → Supplier adatok — *CodeGen-CSharp*
* [x] Magyar számnév konverzió `AmountText` — *CodeGen-CSharp*
* [x] NavigationService unit tesztelése — *TestWriter*

---

## 🔍 M7 – Lookup Integration

* [x] Lookup dialógusok (F2 / Ctrl+L) — *CodeGen-XAML*
* [x] UX flow dokumentálása — *ux\_agent*
* [x] Ár-előzmények mentése JSON-ben — *CodeGen-CSharp*
* [x] Ármemória teszt — *TestWriter*

---

## 🎯 M8 – EF Core Migration

* [x] EF Core váltás, `WreceptDbContext` — *CodeGen-CSharp*
* [x] DI újrainjektálás — *Architect*
* [x] Repository tesztek frissítése — *TestWriter*
* [x] ORM váltás dokumentálása — *DocWriter*

---

## 🧩 M9 – DI Container Finalization

* [x] `AppContext` kiváltása — *CodeGen-CSharp*
* [x] DI-re szabott ViewModel konstrukciók — *CodeGen-CSharp*
* [x] Tesztalkalmazás új DI modell szerint — *TestWriter*
* [x] DI rendszer dokumentálása — *DocWriter*

---

> 📎 Minden feladathoz a `docs/progress/` mappában kell naplót létrehozni `UTC timestamp + agent` névvel.
