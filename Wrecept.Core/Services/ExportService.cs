using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Wrecept.Core.Data;
using Wrecept.Core.Models;

namespace Wrecept.Core.Services;

public class ExportService : IExportService
{
    private readonly AppDbContext _db;
    public ExportService(AppDbContext db)
    {
        _db = db;
    }

    public async Task ExportAsync(string path)
    {
        var dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
        var data = new ExportData
        {
            Products = await _db.Products.AsNoTracking().ToListAsync(),
            Suppliers = await _db.Suppliers.AsNoTracking().ToListAsync(),
            Invoices = await _db.Invoices.Include(i => i.Items).AsNoTracking().ToListAsync()
        };
        var json = JsonSerializer.Serialize(data);
        await File.WriteAllTextAsync(path, json);
    }

    public async Task ImportAsync(string path)
    {
        if (!File.Exists(path)) throw new FileNotFoundException(path);
        var json = await File.ReadAllTextAsync(path);
        var data = JsonSerializer.Deserialize<ExportData>(json);
        if (data is null) return;
        _db.Products.RemoveRange(_db.Products);
        _db.Suppliers.RemoveRange(_db.Suppliers);
        _db.Invoices.RemoveRange(_db.Invoices);
        await _db.SaveChangesAsync();
        if (data.Products != null) _db.Products.AddRange(data.Products);
        if (data.Suppliers != null) _db.Suppliers.AddRange(data.Suppliers);
        if (data.Invoices != null) _db.Invoices.AddRange(data.Invoices);
        await _db.SaveChangesAsync();
    }

    private class ExportData
    {
        public List<Product>? Products { get; set; }
        public List<Supplier>? Suppliers { get; set; }
        public List<Invoice>? Invoices { get; set; }
    }
}
