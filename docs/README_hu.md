---
title: "Wrecept README (HU)"
purpose: "Projekt áttekintés"
author: "root_agent"
date: "2025-06-27"
---

# 🎛️ Wrecept

**Retro modern számlázó alkalmazás, Windowsra, WPF alapon.**

## ✨ Funkciók (tervezett)

| Funkció                           | Állapot                |
| --------------------------------- | ---------------------- |
| Retro-stílusú UI                  | ✅ Stage felület kész   |
| Strukturált felső menü            | ✅ StageView elérhető   |
| Számlarögzítés (fej/sor)          | ⏳ UI váz készen        |
| Terméktörzs                       | ⏳ Lista nézet elérhető |
| Szállító kiválasztás              | ⏳ Lista nézet elérhető |
| SQLite alapú perzisztencia        | 🔒 Halasztva            |
| Audiovizuális visszajelzés        | 🔒 Halasztva            |
| Mentés áramszünet után            | 🔒 Halasztva            |

---

## 📁 Könyvtárstruktúra

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

## ✅ Tesztek futtatása

A tesztek a következő paranccsal indíthatók:

```bash
dotnet test tests/Wrecept.Tests/Wrecept.Tests.csproj
```

---

További részletek: [manuals/developer_guide_hu.md](manuals/developer_guide_hu.md) és [manuals/user_manual_hu.md](manuals/user_manual_hu.md).

*Készült: 2025-07-07 – fordító: Fecó*

