using Wrecept.Core.Entities;

namespace Wrecept.Core.Repositories;

public interface IProductGroupRepository
{
    Task<List<ProductGroup>> GetAllAsync(CancellationToken ct = default);
}
