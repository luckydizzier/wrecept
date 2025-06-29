---
title: "Wrecept README"
purpose: "Project overview"
author: "root_agent"
date: "2025-06-27"
---

# 🎛️ Wrecept

**A retro-modern invoice recording desktop application inspired by DOS-era logistics systems.**

---

## 📦 Project Purpose

Wrecept eredetileg Windowson futó WPF alkalmazásként indult. A multiplatform MAUI kitérő után ismét WPF-re építjük a felületet, megőrizve a Clipper + dBase IV rendszerek sebességét és tiszta billentyűs vezérlését (Enter / Esc / ↑ / ↓). A cél továbbra is a kiszámítható működés, akár áramszünet után is.

---

## ✨ Features (Planned)

| Feature                          | Status                  |
| -------------------------------- | ----------------------- |
| Retro-style UI                   | ✅ Stage design complete |
| Keyboard-only control            | ⏳ Logic in progress     |
| Structured top menu              | ✅ Visual demo only      |
| Invoice recording (header/items) | ⏳ Coming soon           |
| Product master data              | ⏳ Basic editing available |
| Supplier selection               | ⏳ Planned               |
| SQLite-powered persistence       | 🔒 Deferred             |
| Audio-visual feedback            | 🔒 Deferred             |
| Backup & recovery after outage   | 🔒 Deferred             |

---

## 🎹 Interface Philosophy

* **No mouse. No clutter.**
* Only `Enter`, `Esc`, `↑`, `↓` keys are used.
* All screens mimic DOS layouts — with color-coded panels, keyboard footer hints, and full-screen efficiency.
* Menük nagy része még helykitöltő, de a termékek kezelése már működik.

---

## 📁 Folder Structure

```
<tervezett WPF projekt>
├── App.xaml                          # Alkalmazás definíció
├── MainWindow.xaml                   # Főablak
├── Views/                            # XAML nézetek
├── Themes/RetroTheme.xaml            # Retro színséma
├── Assets/                           # Ikonok, hangok
└── README.md
```

---

## 🛠 Technologies

* Language: **C#** (.NET 8)
* UI: **WPF (.NET 8)**
* Platform: **Windows**

---

## 🎯 Next Steps

1. Build out invoice editor UI (inspired by screenshot #3)
2. Add keyboard navigation logic layer (Enter / Esc focus cycle)
3. Integrate fake data into product and supplier modules
4. Discuss minimal database model based on real .dbf structure
5. Kötelező induló tennivalók a [DEV_SPECS.md](DEV_SPECS.md) "Kick OFF" szakaszában

## ✅ Kick OFF

A WPF projekt a jövőben `Wrecept.Wpf` néven jön létre, és a tervek szerint az alábbi alapelemeket tartalmazza:

* `App.xaml` és `App.xaml.cs` – alkalmazásbeállítások
* `MainWindow.xaml` – főablak
* `App.xaml.cs` tartalmazza a DI és indítási logikát

Ezek garantálják, hogy a program Windows környezetben azonnal futtatható legyen.

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
