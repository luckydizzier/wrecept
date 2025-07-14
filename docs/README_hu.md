---
title: "Wrecept README (HU)"
purpose: "Projekt áttekintés"
author: "root_agent"
date: "2025-06-27"
---

# 🎛️ Wrecept

**Retro modern számlázó alkalmazás, .NET MAUI keretrendszeren.**

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

## ⌨️ Billentyű-navigáció

A billentyűzet kezelése minden támogatott rendszeren egységes. A globális
`KeyboardNavigator` osztály parancsokra fordítja a leütéseket és kezeli a fókuszt.

| Gyorsbillentyű | Művelet |
| -------------- | --------------------------- |
| **F1**         | Fókusz a menüsávon |
| **F2**         | Új számla létrehozása |
| **F3**         | Kijelölt számla szerkesztése |
| **Ctrl+F**     | Keresőmező aktiválása |

Az `Enter` a következő mezőre lép, az utolsó mezőben pedig ment. Az `Esc`
mindig bezárja az aktuális párbeszédet vagy megszakítja a szerkesztést.

---

## 📁 Könyvtárstruktúra

```
InvoiceApp.Core/       # Domain modellek és szolgáltatások
InvoiceApp.Data/       # EF Core adatkezelés és repositoryk
InvoiceApp.MAUI/       # MAUI UI projekt
docs/                  # Dokumentációk
tools/                 # Segédszkriptek
CHANGELOG.md
InvoiceApp.sln
```

---

## ✅ Tesztek futtatása

A tesztek a következő paranccsal indíthatók:

```bash
dotnet test tests/InvoiceApp.Core.Tests/InvoiceApp.Core.Tests.csproj
dotnet test tests/InvoiceApp.MAUI.Tests/InvoiceApp.MAUI.Tests.csproj
```

## 📦 Csomagolás

Windowsra MSIX telepítő készíthető az alábbi parancssal:

```bash
dotnet publish InvoiceApp.MAUI -f net8.0-windows10.0.19041.0 -c Release \
  -p:WindowsPackageType=MSIX
```

macOS és Linux esetén a szokásos `dotnet publish` használható:

```bash
dotnet publish InvoiceApp.MAUI -f net8.0-maccatalyst -c Release
dotnet publish InvoiceApp.MAUI -f net8.0-linux -c Release
```

---

További részletek: [manuals/developer_guide_hu.md](manuals/developer_guide_hu.md) és [manuals/user_manual_hu.md](manuals/user_manual_hu.md).

*Készült: 2025-07-07 – fordító: Fecó*

