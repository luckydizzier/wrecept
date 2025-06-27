---
title: "Test Matrix"
purpose: "Use-case coverage for reliability"
author: "docs_agent"
date: "2025-06-27"
---

# 🧪 Test Matrix

| Szituáció        | Elvárt viselkedés                                                                 |
|------------------|----------------------------------------------------------------------------------|
| UI lock          | Az alkalmazás újra fókuszálja az aktív elemet vagy visszaáll alapállapotba.       |
| Network loss     | A lokális műveletek zavartalanul folytatódnak, figyelmeztető üzenet jelenik meg. |
| Corrupted invoice| A betöltés megszakad, a hibát naplózzuk és a felhasználót tájékoztatjuk.          |
