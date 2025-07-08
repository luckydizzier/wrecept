UI_FLOW.md

🧱 Áttekintés

Ez a dokumentum a Wrecept felhasználói felületének működési folyamatát írja le. Részletezi a navigációs modellt, az elvárt viselkedéseket és az adatbeviteli lépéseket, az inline szerkesztés támogatásával összhangban.

📌 Navigációs modell

Indításkor üres képernyő jelenik meg, a felső menüsor fókusza a Számlák fülön áll.

Menükezelés:

Balra/Jobbra nyíl – váltás a főmenük között: Számlák, Törzsek, Listák, Karbantartás, Névjegy, Kilépés

Fel/Le nyíl – az almenüpontok közti mozgás

Enter – megnyitja a kijelölt nézetet, az első mezőre állítva a fókuszt

Esc – visszalép a menübe az utoljára kiválasztott elemmel

AccessKey jelek (aláhúzás) segítik az Alt+betű kombinációkat a főmenüben és a
számlafejléc mezőknél. A fókusz és a billentyűk útját központi
`FocusManager` és `KeyboardManager` szolgáltatások rendezik, így a nézetek csak
a kezdő elemüket regisztrálják.

A billentyűk feldolgozását állapottól függő handlerek végzik:
- **StageMenuKeyboardHandler** – a főmenüben Up/Down mozgatja a menüpontokat,
  Insert vagy Enter aktiválja a kijelölt műveletet.
- **InvoiceEditorKeyboardHandler** – számlaszerkesztéskor az Insert új sort hoz
  létre, Delete archiválást kér, Enter menti a sort, Escape kilép a szerkesztésből.
- **MasterDataKeyboardHandler** – a `StageViewModel.CurrentViewModel` értékét
  `IEditableMasterDataViewModel`-ként kezeli. Insert/Enter a szerkesztést,
  Delete a törlést, Escape a részletek bezárását indítja.
  A handler példányok DI-n keresztül ugyanazt a ViewModel objektumot kapják, mint a hozzá tartozó nézetek.

- Fókuszkezdő pontok nézetenként:
  - **StageView** – a főmenüsor első eleme
  - **InvoiceLookupView** – `InvoiceList` `ListBox`
    - Betöltés után a `FocusManager.RequestFocus(InvoiceList)` hívás helyezi a
      kurzort a listára.
  - **InvoiceEditorLayout** – bal oldali `InvoiceList`
  - **ProductMasterView** – a táblázat (Grid)
  - **SupplierMasterView** – a táblázat (Grid)
  

🧾 Számlaszerkesztési folyamat (Bejövő szállítólevelek)

1. Számlaszám mező (keresés és létrehozás)

A felső AutoCompleteBox időrendben mutatja a meglévő számlaszámokat.

Insert lenyomására vagy a lista tetejére lépve az `INumberingService` automatikusan új számlaszámot ad, majd a szerkesztő nézetre vált.

2. Számlafejléc adatok

A számlaszám megerősítése után:

- Szállító kiválasztása (LookupBox)
- Dátum megadása (alapértelmezett = mai nap)
- Fizetési mód (LookupBox)
- Bruttó jelölőnégyzet a számlaszám mellett, csak szerkeszthető számlánál aktív.

3. Tételsorok bevitele

- LookupBox valós idejű szűréssel
- Ha nincs találat, felugró űrlappal vehető fel új entitás
- Mennyiség, ár és ÁFA a legutóbbi használatból előtöltve
- Szabad szöveges megjegyzés mező soronként
- Enterrel menthető a sor, Esc lemondja
- Új sor Inserttel
- Escape esetén a rendszer rákérdez: "Végeztél a szerkesztéssel? (Enter/Esc)". Enter ment, Esc visszalép az utolsó mezőre.
- Negatív mennyiség visszárut jelez, piros kiemeléssel.
- Nulla mennyiséget a program nem fogad el, hibaüzenetet mutat.

📄 Számla véglegesítése

„PDF mentés” és „Nyomtatás” gomb minden számlánál elérhető

Archivált számlák:

- Nem szerkeszthetők
- Nem adhatók hozzá vagy törölhetők sorok
- Csak olvasható mezők jelennek meg

Archiválás gomb csak nem archivált számlánál jelenik meg. Jóváhagyáskor figyelmeztető üzenet: "Véglegesíti a számlát, később nem módosítható."

📊 LookupBox működés

Minden törzsadat mező (Szállító, Termék, ÁFA, Mértékegység) egységes LookupBox komponenst használ.

Gépelés közben valós időben szűri a listát.

A fel/le nyilak a szűrt lista elemei között mozognak.

Enter elfogadja a kijelölt elemet és a következő mezőre ugrik.

Ha nincs találat és Entert nyomunk, felugró űrlap jelenik meg (InlineCreatorViewModel).

Escape megszakítja a szerkesztést vagy bezárja az inline űrlapot.

Példa:

→ gépeljük be: "tri..."
→ Találatok: "Trappista", "Trikolor paprika", stb.
→ ↓ választja: "Trappista"
→ Enter → ProductId = 23 lesz beállítva

📀 Képernyővázlatok

🔳 Főmenü folyamat

┌────────────────────────────────────────────────────────────────────────────┐
│ [Számlák] [Törzsek] [Listák] [Karbantartás] [Névjegy] │
│                                                      │
│ > Bejövő szállítólevelek                             │
│   Termékek                                           │
│   Szállítók                                          │
│   ...                                                │
└───────────────────────────────────────────────────────────────────────┘

🧾 Számlaszerkesztő nézet

┌───── Lista ────┬──────── Számla szerkesztő ────────────────┐
│ [Számlaszám]   │ Szállító: [LookupBox   ]               │
│ [Dátum]        │ Dátum:    [2025-06-30  ]                │
│ [Szállító]     │ Szám:     [XXXXX1231   ]                │
│                │ Fiz. mód: [LookupBox   ]   [ ] Bruttó │
│                ├──────────────────────────────────────────┤
│                │ Termék  Menny. Csop. Me.e. Ár  ÁFA       │
│                │ [Edit] [  1] ...                         │
│                │ ... korábban felvitt sorok ...           │
│                │ Enter az első sorban ment és töröl      │
│                │ Esc bármikor visszaugrik a bal oldali    │
│                │ listára                                  │
└────────────────┴──────────────────────────────────────────┘

🔁 Speciális viselkedés

- Minden nézet a StageView keretében töltődik be, így nincsenek modális megszakítások.
- Az inline űrlapok nem vihetik el a fókuszt az aktuális nézetből.
- A menüállapot Esc lenyomása után is megőrzi az utolsó fókuszpozíciót.

📚 Jövőbeli listanézetek

A menük később listázásokat (például számlatörténet, termékforgalom) töltenek majd be saját moduljaikból.

Rácsos listák kialakítása most még nem szükséges, később bővíthető.

📌 Korlátozások

- Archiválás csak üzleti szabályok szerint történhet, utólag nem módosítható.
- A Bruttó jelölés végig meghatározza az árképzést.
- A felületnek mindig tükröznie kell, mely műveletek érhetők el az aktuális állapotban.

ℹ️ Ez a fájl a BUSINESS_LOGIC.md és a RefactorPlan.md dokumentummal együtt alkot egységes leírást a UI viselkedéséről.

📺 Képernyőmód beállító ablak

A "Karbantartás / Képernyő" menüpont egy kis modális ablakot nyit meg. A `ScreenModeWindow` egy egyszerű `ListBox`-ot tartalmaz, amely az elérhető módokat sorolja fel. Az "OK" gomb az aktuális kiválasztást menti és lezárja az ablakot, a "Mégse" visszalépésre szolgál.

A `ScreenModeViewModel` tölti be az értékeket az `Enum.GetValues<ScreenMode>()` hívással. A `SelectedMode` tulajdonság az `ObservableProperty` attribútumot használja, a `RelayCommand` pedig meghívja a `ScreenModeManager.ChangeModeAsync` metódust. A ViewModel az ablak `DataContext`-jeként működik.

📐 `ScreenModeManager` szerepe

Induláskor a `ScreenModeManager.ApplySavedAsync` kiolvassa a `%AppData%/Wrecept/settings.json` fájlt a `SettingsService` segítségével. A beállított ablakméret és betűméret így visszaáll az előző állapotra. Az új mód kiválasztásakor a szolgáltatás frissíti a főablak méreteit, majd elmenti az értéket a `settings.json`-ba az `ISettingsService.SaveAsync` hívással.

📋 Dialóguskezelés lépései

1. A ViewModel létrehozza a megfelelő szerkesztő ViewModelt (pl. `ProductEditorViewModel`).
2. Az `OnOk` delegáltban frissíti a kiválasztott entitást, majd meghívja a szolgáltatást a mentésre.
3. A `DialogService.EditEntity<TView, TViewModel>` hívás elkészíti az `EditEntityDialog` példányt, és átadja az `Ok`/`Mégse` parancsokat.
4. A `NavigationService.ShowCenteredDialog` megjeleníti a dialógust a főablak közepén. A `DialogHelper` gondoskodik a billentyűk leképezéséről.

### Tulajdonosi adatok felvétele

Első indításkor a `UserInfoWindow` kéri be a cég adatait. A mezők kötelezőek, az
utolsó
mező után az `OK` gombra kerül a fókusz, majd megerősítő üzenet jelenik meg:
"Helyesek az adatok?". `Enter` elfogadja, `Escape` az előző mezőre visz
vissza. Minden üres mező piros keretet kap, amíg ki nem töltik.

Későbbi módosításhoz a *Szerviz / Tulajdonos szerkesztése...* menüpont ugyanazt
a `UserInfoWindow` párbeszédet nyitja meg. A mentés után a háttérben
`UserInfoService.SaveAsync` frissíti a `wrecept.json` fájlt, majd a
`UserInfoViewModel` értékei is aktualizálódnak.
