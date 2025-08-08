using System.Collections.Generic;
using System.Threading.Tasks;
using Wrecept.Core.Services.Dtos;

namespace Wrecept.Core.Services;

public interface IAnalyticsService
{
    Task<IReadOnlyList<MonthlyRevenueDto>> GetMonthlyRevenueAsync(int year);
    Task<IReadOnlyList<TopSupplierDto>> GetTopSuppliersAsync(int topN);
    Task<TaxBreakdownDto> GetTaxBreakdownAsync();
}
