# Codex Audit Report – 2025-06-30

## ✅ Passed Checks

- Minden modell tartalmazza az `CreatedAt` és `UpdatedAt` mezőket.
- `Invoice`, `InvoiceItem`, `Product`, `Supplier` és `PaymentMethod` modellek megfelelnek a RefactorPlan szerinti mezőknek.
- `InvoiceCalculator` helyesen kezeli az `IsGross` logikát és a negatív mennyiségeket.
- `PaymentMethodService.GetActiveAsync` használata biztosítja a nem archivált elemek szűrését a számlaszerkesztőben.

## ⚠️ Issues Detected

- A `TaxRate.Code` és `Unit.Code` mezők nem opcionálisak, holott a tervben `string?` szerepel.
- Számos ViewModel nem tükrözi a kibővített modelleket:
  - `InvoiceEditorViewModel` nem tartalmaz dátumot, beszállító ID-t és tétel szintű TaxRate kezelést.
  - A törzsadatok ViewModeljei (`ProductMasterViewModel`, `SupplierMasterViewModel`, stb.) minden rekordot betöltenek, archiválási szűrő nélkül.
- A hozzá tartozó XAML nézetek hiányosak:
  - `InvoiceEditorView` nem rendelkezik dátumválasztóval és beszállító-választó ComboBox-szal.
  - A tételsorok nem kezelik a TaxRate felülbírálását, illetve a negatív mennyiségek vizuális jelzését.
  - `TaxRateMasterView` és `UnitMasterView` nem jelenítik meg a `Code` mezőt.
- A legtöbb lista `GetAllAsync` hívást alkalmaz, így archivált rekordok is megjelennek.

## 💡 Suggested Fixes

- Módosítsuk a `TaxRate` és `Unit` modellek `Code` mezőit opcionálisra, ha nincs rá üzleti kényszer.
- Egészítsük ki az `InvoiceEditorViewModel`-t a hiányzó mezőkkel (dátum, beszállító ID, TaxRate választás).
- Használjuk a `GetActiveAsync` metódusokat minden törzsadat ViewModelben, és frissítsük a nézeteket ennek megfelelően.
- Bővítsük a XAML felületeket a hiányzó mezőkkel és vizuális visszajelzésekkel (pl. negatív mennyiségek piros kiemelése).

