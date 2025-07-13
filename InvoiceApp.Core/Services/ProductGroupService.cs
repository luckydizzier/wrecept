using InvoiceApp.Core.Models;
using InvoiceApp.Core.Repositories;

namespace InvoiceApp.Core.Services;

public class ProductGroupService : IProductGroupService
{
    private readonly IProductGroupRepository _groups;

    public ProductGroupService(IProductGroupRepository groups)
    {
        _groups = groups;
    }

    public Task<List<ProductGroup>> GetAllAsync(CancellationToken ct = default)
        => _groups.GetAllAsync(ct);

    public Task<List<ProductGroup>> GetActiveAsync(CancellationToken ct = default)
        => _groups.GetActiveAsync(ct);

    public async Task<Guid> AddAsync(ProductGroup group, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(group);
        if (string.IsNullOrWhiteSpace(group.Name))
            throw new ArgumentException("Name required", nameof(group));

        group.CreatedAt = DateTime.UtcNow;
        group.UpdatedAt = DateTime.UtcNow;
        return await _groups.AddAsync(group, ct);
    }

    public async Task UpdateAsync(ProductGroup group, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(group);
        if (group.Id == Guid.Empty)
            throw new ArgumentException("Invalid Id", nameof(group));
        if (string.IsNullOrWhiteSpace(group.Name))
            throw new ArgumentException("Name required", nameof(group));

        group.UpdatedAt = DateTime.UtcNow;
        await _groups.UpdateAsync(group, ct);
    }
}
