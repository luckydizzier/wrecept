---
title: "Fault Injection Plan"
purpose: "Scenarios and recovery steps for reliability testing"
author: "root_agent"
date: "2025-06-27"
---

# 🐞 Fault Injection Plan

Ez a dokumentum meghatározza, hogyan vizsgáljuk az alkalmazás hibával szembeni ellenállását.

## Tesztesetek

1. **Adatbázis kapcsolat megszakadása** – Szimuláljuk az elérhetetlenséget és figyeljük, hogy a Storage réteg hogyan kezeli a tranzakciók visszagörgetését.
2. **Hiányzó vagy sérült konfigurációs fájl** – Indításkor ellenőrizzük, hogy a beállítások olvashatók-e; hiba esetén alapértelmezett értékeket töltünk.
3. **Nem várt kivétel a ViewModelben** – Ellenőrizzük, hogy az Error Handling terv szerint logol-e és jelzi-e a hibát a felhasználónak.

## Cél

A hibabefecskendezés révén igazolni tudjuk, hogy a rendszer hiba esetén is kontrolláltan működik tovább vagy elegánsan leáll.

---
