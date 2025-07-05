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
   * beérkezés dátuma alapján **automatikusan számítható a fizetési határidő**,
     amelyet az `Invoice.DueDate` mező tárol.

   A fejlécadatok először csak memóriában kerülnek tárolásra. Az adatbázisba írás
   a teljes számla mentésekor történik, így elkerülhetők a félbehagyott rekordok.

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
  * A Unit entitás a termékek mértékegységét tárolja (pl. "kg", "db"), archiválható.
* A rendszer moduláris felépítésű, bővíthető:

  * `InvoiceService` üzleti logika,
  * `InvoiceCalculator` számítási egység,
  * `InvoiceFormatter` nyomtatáshoz,
  * `ReportService` jövőbeni riportokhoz.
* Lehetőség van több felhasználós támogatásra `CreatedBy` mezőkkel.

---

11. 🧮 InvoiceCalculator – Számlaösszesítő logika
Az InvoiceCalculator komponens felelős a számlatételek alapján az összesített értékek kiszámításáért, figyelembe véve a bruttó/nettó logikát, ÁFA kulcsokat és a visszárukat.

11.1. Bemenetek
Invoice objektum, amely tartalmazza:

IsGross beállítást (bruttó/nettó megadás),

InvoiceItems listáját (tételsorok),

minden tételhez kapcsolt Product, benne a TaxRate hivatkozással.

11.2. Kimenetek
TotalNet: az összes nettó összeg (negatív sorokat beleszámítva),

TotalTax: összesített ÁFA összeg,

TotalGross: teljes bruttó érték,

PerTaxRateBreakdown: kulcs-érték lista, ahol minden ÁFA-kulcshoz külön:

nettó összeg,

ÁFA összeg,

bruttó összeg tartozik.

11.3. Számítási algoritmus
Tételsoronként:

Egységár értelmezése:

Ha IsGross = false: UnitPrice = nettó

Ha IsGross = true: UnitPrice = bruttó, ezért:


NetUnitPrice = UnitPrice / (1 + TaxRate.Percentage / 100)
Tételérték számítása:


NetAmount = Quantity * NetUnitPrice
TaxAmount = NetAmount * (TaxRate.Percentage / 100)
GrossAmount = NetAmount + TaxAmount
Negatív mennyiség esetén:

Minden érték (nettó, áfa, bruttó) negatívként kerül be az összesítőbe.

Ez jellemzően visszáru (pl. -2 db termék).

Csoportosítás:

A TaxRate.Id alapján minden érték csoportosításra kerül:

Összes nettó, összes áfa, összes bruttó kulcsonként.

Összesítő értékek:

TotalNet = Σ(NetAmount)

TotalTax = Σ(TaxAmount)

TotalGross = Σ(GrossAmount)

11.4. Kerekítés
Minden számítás belül decimálisan történik (decimal típus).

Kimenetek megjelenítés előtt kerülnek pénznem szerint formázásra (pl. #,##0 Ft).

11.5. Hibalogika
Ha a tétel Product vagy TaxRate hiányzik → kivétel vagy validációs hiba.

Ha Quantity = 0 → kihagyható a számításból, de figyelmeztetés jelenhet meg.

