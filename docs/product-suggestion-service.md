# Product Suggestion Service

The `ProductSuggestionService` builds ranked product suggestions based on recent invoice items. It queries `IRepository<InvoiceItem>` and orders products by most recent usage and frequency.

```csharp
var service = new ProductSuggestionService(invoiceItemRepository);
var suggestions = await service.GetSuggestionsAsync("milk");
```

Use the `searchTerm` to filter by product name. The service returns products ordered by most recent invoice date and frequency.
