---
title: "Tesztmátrix"
purpose: "Használati esetek lefedettsége a megbízhatósághoz"
author: "docs_agent"
date: "2025-06-27"
---

# 🧪 Tesztmátrix

| Szituáció        | Elvárt viselkedés                                                                 |
|------------------|----------------------------------------------------------------------------------|
| UI zárolás       | Az alkalmazás újra fókuszálja az aktív elemet vagy visszaáll alapállapotba.       |
| Hálózati kiesés  | A lokális műveletek zavartalanul folytatódnak, figyelmeztető üzenet jelenik meg. |
| Sérült számla    | A betöltés megszakad, a hibát naplózzuk és a felhasználót tájékoztatjuk.          |
| WPF build Windows alatt | Ha a .NET Desktop Runtime nincs telepítve, a build nem fut le; ezt naplózzuk. |
| StageView menü | Escape visszaadja a fókuszt az utolsó menüpontra. |
| InvoiceEditor sor mentése | Új tétel rögzítését követően frissül a lista. |
| Masteradat ablakok | A rács fókuszálható és Enter szerkesztést indít. |
| ScreenMode váltás | A kiválasztott mód elmentésre kerül és az ablak bezár. |
