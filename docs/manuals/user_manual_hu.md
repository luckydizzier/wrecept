---
title: "Felhasználói kézikönyv"
purpose: "Útmutató a végfelhasználóknak"
author: "root_agent"
date: "2025-06-27"
---

# 📗 Felhasználói kézikönyv

Ez a dokumentum bemutatja az InvoiceApp telepítésének és használatának alapjait.

## Telepítés és első indítás

1. Töltsd le a kiadott csomagot, majd csomagold ki egy tetszőleges mappába.
2. Győződj meg róla, hogy a **.NET Desktop Runtime 8** elérhető a gépen.
3. Indítsd el az MAUI alkalmazás futtatható állományát (például `InvoiceApp.MAUI.exe`). Az első indításkor megadhatod a konfigurációs fájl helyét, vagy elfogadhatod az alapértelmezést.

## Alapvető navigáció

- A felső menüsor elemei között a **bal** és **jobb** nyíllal lépkedhetsz.
- **Le** és **fel** nyíllal választhatod ki az almenüket, **Enter** billentyűvel nyithatod meg őket.
- **Esc** visszalép a főmenübe.

Példa: a *Törzsek > Termékek* menüpont megnyitásához jobbra lépegetve válaszd a *Törzsek* fület, majd lefelé a *Termékek* sort és nyomj **Enter**.

## Alap feladatok

1. **Új termék rögzítése**
   - Nyisd meg a *Törzsek > Termékek* menüpontot.
   - A megjelenő listában nyomd meg az **Insert** billentyűt, töltsd ki a mezőket, majd erősítsd meg **Enter**-rel.
2. **Számla rögzítése**
   - Válaszd a *Számlák > Bejövő szállítólevelek* opciót.
   - A bal oldali listában hozd létre az új bizonylatot, majd add hozzá a tételeket.

## Karbantartás

A *Szerviz* menü segít az adatállományok karbantartásában:

- **Állományok ellenőrzése** – adatbázis-hiba esetén futtasd.
- **Áramszünet után** – nem várt leállás után indítsd.
- **Mentés** – a teljes adatbázist és beállításokat ZIP fájlba menti.
- **Visszaállítás** – korábbi mentésből tölti vissza az adatokat.
- **Tulajdonos szerkesztése...** – itt módosíthatod a céges adatokat, amelyek a számlák fejlécében megjelennek.


