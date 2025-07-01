---
title: "WPF Themes"
purpose: "Style guide for RetroTheme"
author: "docs_agent"
date: "2025-06-27"
---

# 🎨 Retro Theme Overview

A Retro UI hangs on a 16 színű DOS-palettán. A XAML erőforrás szótár egységesíti a vezérlők megjelenését. A fő színek: háttér `#9b8b20`, mezők `#c7bb4f`, gombok `#0000aa` fehér felirattal és kiemelés `#000055`.

2025 júliusától a `RetroTheme.xaml` két változatban érhető el: világos és sötét. A váltást a `ThemeManager.ApplyDarkTheme(bool)` hívással lehet vezérelni, ami a megfelelő `ResourceDictionary` betöltését végzi.

- **StageBackground:** Mustár sárga (`#9b8b20`) alap minden ablakhoz.
- **HighlightBrush:** `#0000aa` a gombokhoz és aktív elemekhez.
- **SelectionBrush:** `#000055` kijelölt sorokhoz.
- **ControlBackgroundBrush:** Olívzöld (`#c7bb4f`) mezők alapja.
- **HeaderFooterBrush:** Sötétebb sárga (`#806000`) fejléc és lábléc szín.

Betűméretek:
- **FontSizeNormal:** 16 px, általános szövegekhez és űrlapmezőkhöz.
- **FontSizeLarge:** 18 px, gombokhoz, menükhöz és táblázatokhoz.

DataGrid sorok fekete háttérrel és arany kiemeléssel jelennek meg a DOS-hatást erősítve. A mennyiségi vagy ár mezők címkéi piros színt kapnak, hogy gyorsabban felismerhetők legyenek.

Every control style sets `FocusVisualStyle` to display a dashed border so keyboard navigation is obvious.
- The StatusBar uses `ControlBackgroundBrush` with subtle text to avoid distraction while conveying state.
