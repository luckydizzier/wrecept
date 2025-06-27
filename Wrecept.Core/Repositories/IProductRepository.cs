using Wrecept.Core.Models;

namespace Wrecept.Core.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync(CancellationToken ct = default);
}
