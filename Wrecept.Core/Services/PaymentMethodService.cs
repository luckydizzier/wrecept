using Wrecept.Core.Models;
using Wrecept.Core.Repositories;

namespace Wrecept.Core.Services;

public class PaymentMethodService : IPaymentMethodService
{
    private readonly IPaymentMethodRepository _methods;

    public PaymentMethodService(IPaymentMethodRepository methods)
    {
        _methods = methods;
    }

    public Task<List<PaymentMethod>> GetAllAsync(CancellationToken ct = default)
        => _methods.GetAllAsync(ct);

    public Task<List<PaymentMethod>> GetActiveAsync(CancellationToken ct = default)
        => _methods.GetActiveAsync(ct);

    public async Task<Guid> AddAsync(PaymentMethod method, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(method);
        if (string.IsNullOrWhiteSpace(method.Name))
            throw new ArgumentException("Name required", nameof(method));
        if (method.DueInDays < 0)
            throw new ArgumentException("DueInDays cannot be negative", nameof(method));

        method.CreatedAt = DateTime.UtcNow;
        method.UpdatedAt = DateTime.UtcNow;
        return await _methods.AddAsync(method, ct);
    }

    public async Task UpdateAsync(PaymentMethod method, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(method);
        if (method.Id == Guid.Empty)
            throw new ArgumentException("Invalid Id", nameof(method));
        if (string.IsNullOrWhiteSpace(method.Name))
            throw new ArgumentException("Name required", nameof(method));
        if (method.DueInDays < 0)
            throw new ArgumentException("DueInDays cannot be negative", nameof(method));

        method.UpdatedAt = DateTime.UtcNow;
        await _methods.UpdateAsync(method, ct);
    }
}
