# RefactorPlan.md

## 🧠 Objective

This refactor plan addresses the current structure of `Wrecept.Core.Models` and outlines a precise and maintainable roadmap for updating all relevant entities, views, and viewmodels to support business-critical features such as:

* Gross/net invoice calculation
* Product-level TaxRate, Unit, and Group relationships
* Archival support for all master data
* Invoice payment method linkage
* Enhanced support for return items and container logic

> ⚠️ This plan is executable by ChatGPT Codex through the AGENTS.md coordination framework.
> ℹ️ The complete business logic specification is documented in `docs/BUSINESS_LOGIC.md`.

---

## ✅ General Refactor Strategy

All changes should follow the **MVVM pattern**, keeping business logic in services and viewmodels, and minimizing coupling in XAML views. Models must remain pure data holders with appropriate annotations.

Each step includes updates for:

* `Core.Models`
* `ViewModels`
* `Views` (XAML and code-behind)
* Related `Services` if applicable

---

## 1. `Invoice` Enhancements

### Model Changes

```csharp
+ public bool IsGross { get; set; }
+ public Guid PaymentMethodId { get; set; }
+ public PaymentMethod? PaymentMethod { get; set; }
+ public bool IsArchived { get; set; }
```

### ViewModel Changes

* Bind `IsGross` to a Toggle or Checkbox in the invoice editor.
* Bind `PaymentMethodId` using an AutoCompleteBox of active payment methods.
* Add `IsArchived` flag to support filtering/archive logic.

### View (XAML)

* Add gross/net toggle.
* Replace payment method TextBox with AutoCompleteBox.
* Disable editing if `IsArchived` is true.

---

## 2. `InvoiceItem` Enhancements

### Model Changes

```csharp
+ public Guid TaxRateId { get; set; }
+ public TaxRate? TaxRate { get; set; }
```

> Product remains primary, but TaxRate is overridable if needed.

### ViewModel

* Resolve default TaxRate from Product, allow override in advanced mode.
* Validate Quantity (≠ 0) and UnitPrice (≥ 0)

### View

* Optionally add dropdown for TaxRate selection.
* Add logic to mark negative Quantity visually (e.g. red highlight for returns).

---

## 3. `Product` Enhancements

### Model Changes

```csharp
+ public Guid TaxRateId { get; set; }
+ public TaxRate? TaxRate { get; set; }
+ public Guid UnitId { get; set; }
+ public Unit? Unit { get; set; }
+ public Guid ProductGroupId { get; set; }
+ public ProductGroup? ProductGroup { get; set; }
+ public bool IsArchived { get; set; }
```

### ViewModel

* Update `ProductViewModel` to load these relations and persist them.

### View

* Replace text inputs with dropdowns for TaxRate, Unit, Group.
* Add archive toggle.

---

## 4. `Supplier` Enhancements

### Model

```csharp
+ public bool IsArchived { get; set; }
```

### ViewModel & View

* Add archive filter and toggle.
* Prevent selection of archived suppliers in invoice editor.

---

## 5. `TaxRate` Enhancements

### Model

```csharp
+ public string? Code { get; set; } // optional e.g. "A27"
```

### ViewModel & View

* Display Code where relevant (e.g. breakdowns, dropdowns).
* Ensure `EffectiveFrom` and `EffectiveTo` are validated.

---

## 6. `PaymentMethod` Enhancements

### Model

No changes beyond current `DueInDays`, `IsArchived`, etc.

### ViewModel & View

* Make `DueInDays` editable.
* Filter out archived values from Invoice editor AutoCompleteBox.

---

## 7. `Unit` Enhancements

### Model

```csharp
+ public string? Code { get; set; } // e.g. "kg", "pcs"
```

### ViewModel & View

* Show `Code` in dropdowns.
* Ensure archived units are excluded from selection.

---

## 8. `ProductGroup` Enhancements

Already conformant. Just ensure dropdown bindings and filtering work consistently.

---

## 🔁 Change Propagation Notes

* Ensure all AutoCompleteBox elements bind to filtered observable collections (only non-archived items).
* All new IDs must be saved and loaded in ViewModel → Model mapping.
* `InvoiceCalculator` must read `IsGross` and `TaxRate.Percentage` per item.
* Negative `Quantity` must affect all totals.

---

## 🛠 Agent Task Summary

```yaml
- refactor: models/Invoice.cs
- refactor: models/InvoiceItem.cs
- refactor: models/Product.cs
- refactor: models/Supplier.cs
- refactor: models/TaxRate.cs
- refactor: models/PaymentMethod.cs
- refactor: models/Unit.cs
- refactor: viewmodels/**/*ViewModel.cs
- refactor: views/**/*View.xaml
- refactor: views/**/*View.xaml.cs
- update: InvoiceEditor to support gross/net and payment method
- validate: all foreign keys and archive filters
```

---

## ✅ Done Criteria

* Application can correctly distinguish gross vs. net invoices
* Tax breakdowns reflect TaxRate per item
* Archived entities are hidden but preserved
* Views are updated to match extended model data

> AGENTS.md should dispatch these tasks to individual code-generation agents using modular responsibilities.
