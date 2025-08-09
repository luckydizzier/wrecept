namespace Wrecept.Core.Services;

public interface ITaxService
{
    Task<IReadOnlyList<decimal>> GetRatesAsync();
}
