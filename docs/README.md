---
title: "Wrecept README"
purpose: "Project overview"
author: "root_agent"
date: "2025-06-27"
---

# 🎛️ Wrecept

[![CI](https://github.com/luckydizzier/wrecept/actions/workflows/ci.yml/badge.svg)](https://github.com/luckydizzier/wrecept/actions/workflows/ci.yml)

**A retro-modern invoice recording desktop application inspired by DOS-era logistics systems.**

---

## 📦 Project Purpose

Wrecept eredetileg Windowson futó WPF alkalmazásként indult. A multiplatform MAUI kitérő után ismét WPF-re építjük a felületet, megőrizve a Clipper + dBase IV rendszerek sebességét. A cél továbbra is a kiszámítható működés, akár áramszünet után is.

---

## ✨ Features (Planned)

| Feature                          | Status                  |
| -------------------------------- | ----------------------- |
| Retro-style UI                   | ✅ Stage design complete |
| Structured top menu              | ✅ StageView in place    |
| Invoice recording (header/items) | ⏳ UI skeleton ready     |
| Product master data              | ⏳ List view available   |
| Supplier selection               | ⏳ List view available   |
| SQLite-powered persistence       | 🔒 Deferred             |
| Audio-visual feedback            | 🔒 Deferred             |
| Backup & recovery after outage   | 🔒 Deferred             |

---

## 🎹 Interface Philosophy

* **No mouse. No clutter.**
* All screens mimic DOS layouts — with color-coded panels and full-screen efficiency.
* Menük nagy része még helykitöltő, de a termékek kezelése már működik.

---

## 📁 Folder Structure

```
Wrecept.Core/          # Domain modellek és szolgáltatások
Wrecept.Storage/       # EF Core adatkezelés és repositoryk
Wrecept.Wpf/           # WPF UI projekt
docs/                  # Dokumentációk
tools/                 # Segédszkriptek
CHANGELOG.md
Wrecept.sln
```

---

## 🛠 Technologies

* Language: **C#** (.NET 8)
* UI: **WPF (.NET 8)**
* Platform: **Windows**

---

## 🎯 Next Steps

1. Build out invoice editor UI (inspired by screenshot #3)
2. Integrate fake data into product and supplier modules
3. Discuss minimal database model based on real .dbf structure
4. Kötelező induló tennivalók a [DEV_SPECS.md](DEV_SPECS.md) "Kick OFF" szakaszában

## ✅ Kick OFF

A WPF projekt `Wrecept.Wpf` néven jött létre, és az alábbi alapelemeket tartalmazza:

* `App.xaml` és `App.xaml.cs` – alkalmazásbeállítások
* `MainWindow.xaml` – főablak
* `App.xaml.cs` tartalmazza a DI és indítási logikát
* A `MainWindow` betölti a `StageView` felületet

Ezek garantálják, hogy a program Windows környezetben azonnal futtatható legyen.

---

## ✅ Running Tests

A tesztek a következő paranccsal futtathatók:

```bash
dotnet test tests/Wrecept.Tests/Wrecept.Tests.csproj
```

---

## 🧾 Credits

Original layout, logic and color schema: \[Egon’s legacy Clipper app]
Reconstruction by: \[ChatGPT-Dev Agent – 2025 Edition]

> "A színpad áll, a keverő bekészítve. Most jöhet a kábelezés."

---

*Work in Progress – Not intended for production use (yet).*

---

## 📚 Dokumentációk

- [ARCHITECTURE.md](ARCHITECTURE.md) – Rétegek és adatútvonalak
- [AGENTS.md](AGENTS.md) – Agent feladatkiosztás
- [CODE_STANDARDS.md](CODE_STANDARDS.md) – Kódolási irányelvek
- [DEV_SPECS.md](DEV_SPECS.md) – Fejlesztési specifikáció
- [ERROR_HANDLING.md](ERROR_HANDLING.md) – Hibatűrés
- [FAULT_PLAN.md](FAULT_PLAN.md) – Hibabefecskendezési terv
- [TEST_STRATEGY.md](TEST_STRATEGY.md) – Tesztstratégia
