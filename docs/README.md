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

Wrecept is a Windows desktop application designed to replicate the speed, clarity, and simplicity of classic Clipper + dBase IV-based systems, reimagined in C# and WPF. Its UI logic is entirely driven by keyboard navigation (Enter / Esc / Up / Down), and it focuses on rock-solid reliability and predictable behavior — even after a power outage.

---

## ✨ Features (Planned)

| Feature                          | Status                  |
| -------------------------------- | ----------------------- |
| Retro-style UI                   | ✅ Stage design complete |
| Keyboard-only control            | ⏳ Logic in progress     |
| Structured top menu              | ✅ Visual demo only      |
| Invoice recording (header/items) | ⏳ Coming soon           |
| Product master data              | ⏳ To be integrated      |
| Supplier selection               | ⏳ Planned               |
| SQLite-powered persistence       | 🔒 Deferred             |
| Audio-visual feedback            | 🔒 Deferred             |
| Backup & recovery after outage   | 🔒 Deferred             |

---

## 🎹 Interface Philosophy

* **No mouse. No clutter.**
* Only `Enter`, `Esc`, `↑`, `↓` keys are used.
* All screens mimic DOS layouts — with color-coded panels, keyboard footer hints, and full-screen efficiency.
* Menus are *visual placeholders* at this stage. The stage is being built, the cables come later.

---

## 📁 Folder Structure

```
Wrecept.Desktop/
├── App.xaml / MainWindow.xaml        # App entry point and shell
├── Views/
│   ├── StageView.xaml                # Main placeholder canvas
│   ├── MenuBar.xaml                  # Visual top menu only
│   └── StatusBar.xaml                # ESC/Enter hints footer
├── Themes/RetroTheme.xaml           # Retro color scheme
├── Assets/                          # Future icons, sounds, etc.
└── README.md
```

---

## 🛠 Technologies

* Language: **C#** (.NET 8)
* UI: **WPF**
* Platform: **Windows-only** (for now)
* IDE: **Visual Studio 2022+**

---

## 🎯 Next Steps

1. Build out invoice editor UI (inspired by screenshot #3)
2. Add keyboard navigation logic layer (Enter / Esc focus cycle)
3. Integrate fake data into product and supplier modules
4. Discuss minimal database model based on real .dbf structure
5. Kötelező induló tennivalók a [DEV_SPECS.md](DEV_SPECS.md) "Kick OFF" szakaszában

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
