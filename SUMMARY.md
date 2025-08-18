Problem: Invoice status relied solely on color, leaving color-blind and screen-reader users without context.
Approach: Introduced InvoiceStatus with marker/description helpers and bound them in list and detail views.
Files changed:
  - Wrecept.Core/Models/Invoice.cs
  - Wrecept.UI/ViewModels/InvoiceViewModel.cs
  - Wrecept.UI/Views/ListsView.xaml
  - Wrecept.UI/Views/InvoiceView.xaml
  - Wrecept.Core.Tests/InvoiceTests.cs
Risks & mitigations:
  - Marker text hard-coded; localization may be needed later.
Assumptions:
  - Only Active and Inactive statuses are required.
