namespace Wrecept.Core.Services.Dtos;

public record MonthlyRevenueDto(int Month, decimal TotalNet, decimal TotalVat, decimal TotalGross);
