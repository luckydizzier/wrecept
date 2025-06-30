using Wrecept.Core.Models;

namespace Wrecept.Core.Services;

public interface IPaymentMethodService
{
    Task<List<PaymentMethod>> GetAllAsync(CancellationToken ct = default);
    Task<List<PaymentMethod>> GetActiveAsync(CancellationToken ct = default);
    Task<Guid> AddAsync(PaymentMethod method, CancellationToken ct = default);
    Task UpdateAsync(PaymentMethod method, CancellationToken ct = default);
}
