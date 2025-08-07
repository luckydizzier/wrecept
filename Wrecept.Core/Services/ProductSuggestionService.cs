using System;
using System.Collections.Generic;
using System.Linq;
using Wrecept.Core.Models;
using Wrecept.Core.Repositories;

namespace Wrecept.Core.Services;

public class ProductSuggestionService : IProductSuggestionService
{
    private readonly IRepository<InvoiceItem> _invoiceItemRepository;

    public ProductSuggestionService(IRepository<InvoiceItem> invoiceItemRepository)
    {
        _invoiceItemRepository = invoiceItemRepository;
    }

    public async Task<IEnumerable<Product>> GetSuggestionsAsync(string searchTerm, int maxResults = 5)
    {
        var items = await _invoiceItemRepository.GetAllAsync();
        var query = items
            .Where(i => i.Product.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .GroupBy(i => i.Product)
            .Select(g => new { Product = g.Key, LastDate = g.Max(ii => ii.Invoice.Date), Count = g.Count() })
            .OrderByDescending(x => x.LastDate)
            .ThenByDescending(x => x.Count)
            .Take(maxResults)
            .Select(x => x.Product);
        return query;
    }
}
