# BUSINESS\_LOGIC.md – v2.0

## 📍 Cél

Ez a dokumentum definiálja a **Wrecept** rendszer üzleti logikáját. A rendszer egy **egyszerű, egyfelhasználós, offline számlakezelő asztali alkalmazás**, amely lehetővé teszi beérkezett számlák gyors, strukturált rögzítését, azok adataiból statisztikai és üzleti riportok készítését, valamint a törzsadatok karbantartását.

---

## 1. 🔗 Entitáskapcsolatok

* **Supplier** 1\:N **Invoice**
* **Invoice** 1\:N **InvoiceItem**
* **Invoice** N:1 **PaymentMethod**
* **InvoiceItem** N:1 **Product**
* **Product** N:1 **TaxRate**, N:1 **Unit**, N:1 **ProductGroup**

---

## 2. 📋 Számlarögzítés folyamata

1. A felhasználó kiválasztja a **beszállítót**, majd megadja:

   * számlaszámot,
   * kiállítás dátumát,
   * fizetési módot (`PaymentMethod`), amelyhez `DueInDays` is tartozik,
   * beérkezés dátuma alapján **automatikusan számítható a fizetési határidő**.

2. A felhasználó tetszőleges számú tételt rögzít:

   * termék kiválasztása,
   * mennyiség (pozitív vagy negatív, ld. visszáru),
   * egységár,
   * megjegyzés (szabad szöveges mező).

3. A rendszer automatikusan kiszámolja:

   * a tételsor **nettó árát**,
   * az **ÁFA-t** a termékhez rendelt `TaxRate` alapján,
   * a számla összesített értékét **ÁFA-kulcsonként bontva**,
   * végül a **bruttó végösszeget**.

---

## 3. 📊 Összesítés logika

* A számla alján részletes ÁFA-kulcsonkénti bontás jelenik meg:

  ```
  ÁFA 5%  →  nettó: 1000 Ft, ÁFA: 50 Ft
  ÁFA 18% →  nettó: 500 Ft, ÁFA: 90 Ft
  ÁFA 27% →  nettó: 4000 Ft, ÁFA: 1080 Ft
  ```

* **Bruttó összesen**:

  ```
  Bruttó összesen: 6620 Ft
  ```

* **Számmal és betűvel** (pl. "Hat ezerszázhúsz forint") helyes magyar nyelvtani szabályok szerint generálva.

---

## 4. 💶 Bruttó vs. nettó számlák kezelése

* Az `Invoice` entitás tartalmaz egy `IsGross` mezőt, amely meghatározza, hogy a tételekben szereplő egységár:

  * nettó (ha `IsGross = false`) → ÁFA hozzáadódik,
  * bruttó (ha `IsGross = true`) → nettó érték visszaszámításra kerül az ÁFA alapján.

* A kalkuláció minden tételre alkalmazza ezt a szabályt.

---

## 5. 🚚 Visszáru és göngyöleg kezelése

* **Visszáru**:

  * Az `InvoiceItem.Quantity` mező lehet **negatív**, ezzel jelezve, hogy a tétel visszáru.
  * A negatív sor értéke összevonásra kerül a többi tétellel.
  * Az összesítésben szerepel, csökkentve a végösszeget.

* **Göngyöleg**:

  * Olyan `Product`, amely a `ProductGroup`-on keresztül "Göngyöleg" kategóriába tartozik.
  * Így megjeleníthető a számlán, de logikailag elkülöníthető a fő termékektől.
  * Külön jelentésekben szűrhető vagy csoportosítható.

---

## 6. 🚫 Archiválási logika

* A frissen felvitt számlák szerkeszthetőek.
* Archiválás történik manuális eseményre: "Számlák aktualizálása".
* Archivált számlák:

  * nem módosíthatók,
  * nem törölhetők,
  * de bármikor megtekinthetők, kinyomtathatók.
* A folyamat végén a rendszer jelzi az archiválás eredményét.

---

## 7. 💾 Export és nyomtatás

* Számlák PDF-be exportálhatók vagy nyomtathatók.
* Alapértelmezett vevő: **Tankó Ferenc E.V.**, de később konfigurálható.
* Nincs NAV-integráció, nem is tervezett.

---

## 8. 📊 Riportfunkciók (tervezett)

* Termékek és termékcsoportok szerinti összesítés
* Beszállítónkkénti áttekintés
* Időszakos árlista export
* CSV és PDF kimenet

---

## 9. ⚠️ Validációs szabályok

* Számlán legalább egy tételnek szerepelnie kell.
* Tétel: `Quantity != 0`, `UnitPrice ≥ 0`
* Termék, beszállító, fizetési mód kiválasztása kötelező.
* Törzsadatból csak **nem archivált** elemek választhatók.

---

## 10. 🔐 Bővíthetőség

* Törzsadatok (Product, TaxRate, PaymentMethod, ProductGroup, Unit) adminisztrálhatók.
* A rendszer moduláris felépítésű, bővíthető:

  * `InvoiceService` üzleti logika,
  * `InvoiceCalculator` számítási egység,
  * `InvoiceFormatter` nyomtatáshoz,
  * `ReportService` jövőbeni riportokhoz.
* Lehetőség van több felhasználós támogatásra `CreatedBy` mezőkkel.

---
