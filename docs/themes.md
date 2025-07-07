---
title: "WPF témák"
purpose: "Stílus útmutató a RetroTheme-hez"
author: "docs_agent"
date: "2025-06-27"
---

# 🎨 Retro téma áttekintés

A Retro UI hangs on a 16 színű DOS-palettán. A XAML erőforrás szótár egységesíti a vezérlők megjelenését. A fő színek: háttér `#9b8b20`, mezők `#c7bb4f`, gombok `#0000aa` fehér felirattal és kiemelés `#000055`.

2025 júliusától a `RetroTheme.xaml` két változatban érhető el: világos és sötét. A váltást a `ThemeManager.ApplyDarkTheme(bool)` hívással lehet vezérelni, ami a megfelelő `ResourceDictionary` betöltését végzi.

- **StageBackground:** Mustár sárga (`#9b8b20`) alap minden ablakhoz.
- **HighlightBrush:** `#0000aa` a gombokhoz és aktív elemekhez.
- **SelectionBrush:** `#000055` kijelölt sorokhoz.
- **ControlBackgroundBrush:** Olívzöld (`#c7bb4f`) mezők alapja.
- **HeaderFooterBrush:** Sötétebb sárga (`#806000`) fejléc és lábléc szín.
- **DefaultMargin:** 6 px vastag általános margó vezérlők köré.

Stílusok:
- **HeaderText**, **HeaderTextBold**: IBM Plex Mono betűcsalád feliratokhoz.
- **HeaderTextBoxBold**: ugyanez a megjelenés szövegmezőknél.
- **FormLabelStyle**, **LabelTextStyle**, **ValueTextStyle**, **BoldTotalStyle**: számlamezők és összesítések tipográfiája mindkét témában.

Betűméretek:
- **FontSizeNormal:** 16 px, általános szövegekhez és űrlapmezőkhöz.
- **FontSizeLarge:** 18 px, gombokhoz, menükhöz és táblázatokhoz.

DataGrid sorok fekete háttérrel és arany kiemeléssel jelennek meg a DOS-hatást erősítve. A mennyiségi vagy ár mezők címkéi piros színt kapnak, hogy gyorsabban felismerhetők legyenek.

- The StatusBar uses `ControlBackgroundBrush` with subtle text to avoid distraction while conveying state.

## Screen Modes

A `ScreenModeManager` négy előre definiált profil közül választ:
Small (800x600), Medium (1024x768), Large (1280x1024) és ExtraLarge (1920x1080).
Az ablakméret és a betűméret a kiválasztott mód szerint frissül, az értékek a
`%AppData%/Wrecept/settings.json` fájlban tárolódnak.

Az aktuális betűméreteket a `ThemeSizing` osztály állítja be a képernyőmérethez
rendelve:

- **Small:** `FontSizeNormal` 12 px, `FontSizeLarge` 14 px
- **Medium:** `FontSizeNormal` 14 px, `FontSizeLarge` 16 px
- **Large:** `FontSizeNormal` 16 px, `FontSizeLarge` 18 px
- **ExtraLarge:** `FontSizeNormal` 18 px, `FontSizeLarge` 20 px

Ez a beállítás a `ScreenModeManager` hívásakor frissül, így minden nézet kód
módosítás nélkül tud alkalmazkodni a választott módhoz.
