using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Wrecept.Core.Services;
using Wrecept.Core.Services.Dtos;

namespace Wrecept.UI.ViewModels;

public class DashboardViewModel : INotifyPropertyChanged
{
    private readonly IAnalyticsService _analyticsService;

    public ObservableCollection<MonthlyRevenueDto> MonthlyRevenue { get; } = new();
    public ObservableCollection<TopSupplierDto> TopSuppliers { get; } = new();
    public ObservableCollection<TopProductDto> TopProducts { get; } = new();

    public DashboardViewModel(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        var year = DateTime.Now.Year;

        var revenue = await _analyticsService.GetMonthlyRevenueAsync(year);
        MonthlyRevenue.Clear();
        foreach (var r in revenue)
            MonthlyRevenue.Add(r);

        var suppliers = await _analyticsService.GetTopSuppliersAsync(5);
        TopSuppliers.Clear();
        foreach (var s in suppliers)
            TopSuppliers.Add(s);

        var products = await _analyticsService.GetTopProductsAsync(5);
        TopProducts.Clear();
        foreach (var p in products)
            TopProducts.Add(p);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
