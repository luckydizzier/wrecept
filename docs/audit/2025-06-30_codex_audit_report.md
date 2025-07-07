# Codex audit jelentés – 2025-06-30

## ✅ Sikeres ellenőrzések

- Modellek megfelelnek a RefactorPlan előírásainak (`Invoice`, `InvoiceItem`, `Product`, `Supplier`, `TaxRate`, `PaymentMethod`, `Unit`, `ProductGroup`).
- A viewmodel-ek tartalmazzák a szükséges bindolható tulajdonságokat (pl. `InvoiceEditorViewModel` már kezeli a dátumot, szállítót, fizetési módot és bruttó/nettó állapotot).
- A fő XAML nézetek helyesen kötik a mezőket (pl. `InvoiceEditorView` ComboBox-ok és CheckBox a megfelelő propertykre).
- A negatív mennyiségek vizuális jelzése `NegativeValueForegroundConverter` segítségével működik.
- A start folyamatban a `StartupWindow` két ProgressBaron jelzi az előrehaladást.

## ⚠️ Észlelt problémák

- `PaymentMethodMasterViewModel` és `ProductGroupMasterViewModel` még `GetAllAsync` hívást használnak, így az archivált elemek is megjelennek.

## 💡 Javasolt javítások

- Cseréljük `GetActiveAsync` hívásra a fenti viewmodelleket, hogy az archivált rekordok kiszűrve legyenek.

