using Wrecept.Core.Models;

namespace Wrecept.Core.Services;

public interface IProductSuggestionService
{
    Task<IEnumerable<Product>> GetSuggestionsAsync(string searchTerm, int maxResults = 5);
}
