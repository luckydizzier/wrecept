using Wrecept.Core.Entities;

namespace Wrecept.Core.Repositories;

public interface IPaymentMethodRepository
{
    Task<List<PaymentMethod>> GetAllAsync(CancellationToken ct = default);
}
