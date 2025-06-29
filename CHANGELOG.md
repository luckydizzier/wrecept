---
title: "Changelog"
purpose: "Aggregated changes from progress logs"
author: "tools"
date: "2025-06-27"
---

# 📝 Changelog

## 2025-06-27
- Added frontmatter to all documentation files.
- Created missing Hungarian and English manual stubs.
- Initialized progress logging directory.

## 2025-06-27
- Létrehoztam az architektúra, hibakezelés, tesztelés, fault injection és kódstandard dokumentumokat.
- Ezek a hiányzó keretek a projekt működési elveit rögzítik.

## 2025-06-27
- Frissítettem a dokumentációk szerző mezőit a docs_agentre.
- Létrehoztam a FAULT_INJECTION.md fájlt új hibaszcenáriókkal.
- Bővítettem az AGENTS.md-t a docs_agent szerepkörével.

## 2025-06-27
- Kiegészítettem az ERROR_HANDLING.md-t konkrét hibaszcenáriókkal.
- Részletesebbé tettem az adatáramlást az ARCHITECTURE.md-ben.
- Pontosítottam a TEST_STRATEGY.md kódfedettségi és CI szabályait.
- Bővítettem a FAULT_INJECTION.md-t új kockázatokkal.
- Frissítettem a CODE_STANDARDS.md-t null-kezeléssel és naminggel.

## 2025-06-27
- Hivatkozásokat adtam a README.md aljára a fő dokumentumokra.
- Szabályt rögzítettem az AGENTS.md-ben az InvoiceEditor indulásához.
- Verziózási előírással bővítettem a CODE_STANDARDS.md-t.
- Létrehoztam onboarding és Test Matrix dokumentumokat.
- Automatikus changelog generáló script készült.


## 2025-06-28
- A menürendszert visszaállítottam a `fix-almenü-és-menü-hibák` állapotra.
- Eltávolítottam a BuildInfo alapú Névjegy funkciót.

## 2025-06-29
- Visszahoztam a BuildInfo osztályt, a Névjegy menüpont kiírja a build adatait.
- Létrehoztam a Termékek nézetet és ViewModelt.
- A StageViewModel bővült új függőségekkel és megjelenítési logikával.
- A Kilépés menüpont bezárja az alkalmazást.
