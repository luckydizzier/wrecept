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

Wrecept started as a Windows-only desktop application but now aims to run on multiple platforms using .NET MAUI. It replicates the speed and clarity of classic Clipper + dBase IV systems with full keyboard navigation (Enter / Esc / Up / Down) and a focus on predictable behavior — even after a power outage.

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
Wrecept.Maui/
├── App.xaml                          # Application definition
├── MainPage.xaml                     # Cross-platform shell
├── Views/                            # Future XAML pages
├── Themes/RetroTheme.xaml            # Retro color scheme
├── Assets/                           # Icons, sounds, etc.
└── README.md
```

---

## 🛠 Technologies

* Language: **C#** (.NET 8)
* UI: **.NET MAUI**
* Platform: **Cross-platform** (Windows, Android, iOS)
* IDE: **Visual Studio 2022+ / VS Code**

---

## 🎯 Next Steps

1. Build out invoice editor UI (inspired by screenshot #3)
2. Add keyboard navigation logic layer (Enter / Esc focus cycle)
3. Integrate fake data into product and supplier modules
4. Discuss minimal database model based on real .dbf structure
5. Kötelező induló tennivalók a [DEV_SPECS.md](DEV_SPECS.md) "Kick OFF" szakaszában

## ✅ Kick OFF

A .NET MAUI projekt elindításához a `Wrecept.Maui` mappában az alábbi alapfájlok szerepelnek:

* `App.xaml` és `App.xaml.cs` – az alkalmazás beállításai
* `MainPage.xaml` – kezdő nézet
* `MauiProgram.cs` – DI és konfiguráció
* platform-specifikus `Program.cs` a `Platforms/` mappában

Ezek biztosítják, hogy minden támogatott platformon elinduljon az alkalmazás.

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
