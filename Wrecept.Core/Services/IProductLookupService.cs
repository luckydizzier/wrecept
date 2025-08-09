using Wrecept.Core.Models;

namespace Wrecept.Core.Services;

public interface IProductLookupService
{
    Task<IReadOnlyList<Product>> SearchAsync(string term);
}
