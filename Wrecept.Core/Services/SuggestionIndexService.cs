using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Data;
using Wrecept.Core.Models;

namespace Wrecept.Core.Services;

public class SuggestionIndexService : ISuggestionIndexService
{
    private readonly AppDbContext _ctx;

    public SuggestionIndexService(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task AddHistoryEntryAsync(string term)
    {
        if (string.IsNullOrWhiteSpace(term)) return;
        var existing = await _ctx.SuggestionTerms
            .FirstOrDefaultAsync(t => EF.Functions.Collate(t.Term, "NOCASE") == term);
        if (existing == null)
        {
            _ctx.SuggestionTerms.Add(new SuggestionTerm
            {
                Term = term,
                Frequency = 1,
                LastUsedUtc = DateTime.UtcNow
            });
        }
        else
        {
            existing.Frequency++;
            existing.LastUsedUtc = DateTime.UtcNow;
        }
        await _ctx.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<string>> GetPredictionsAsync(string prefix)
    {
        prefix ??= string.Empty;
        var likePrefix = prefix + "%";
        var likeContains = "%" + prefix + "%";

        var fromProductsPrefix = _ctx.Products
            .Where(p => EF.Functions.Like(p.Name, likePrefix))
            .Select(p => new { p.Name, Freq = 0, Last = (DateTime?)null, IsPrefix = true });
        var fromSuppliersPrefix = _ctx.Suppliers
            .Where(s => EF.Functions.Like(s.Name, likePrefix))
            .Select(s => new { Name = s.Name, Freq = 0, Last = (DateTime?)null, IsPrefix = true });
        var fromItemsPrefix = _ctx.InvoiceItems
            .Where(i => EF.Functions.Like(i.Product.Name, likePrefix))
            .GroupBy(i => i.Product.Name)
            .Select(g => new { Name = g.Key, Freq = g.Count(), Last = (DateTime?)g.Max(i => i.Invoice.Date), IsPrefix = true });
        var fromTermsPrefix = _ctx.SuggestionTerms
            .Where(t => EF.Functions.Like(t.Term, likePrefix))
            .Select(t => new { Name = t.Term, Freq = t.Frequency, Last = (DateTime?)t.LastUsedUtc, IsPrefix = true });

        var fromProductsSub = _ctx.Products
            .Where(p => !EF.Functions.Like(p.Name, likePrefix) && EF.Functions.Like(p.Name, likeContains))
            .Select(p => new { p.Name, Freq = 0, Last = (DateTime?)null, IsPrefix = false });
        var fromSuppliersSub = _ctx.Suppliers
            .Where(s => !EF.Functions.Like(s.Name, likePrefix) && EF.Functions.Like(s.Name, likeContains))
            .Select(s => new { Name = s.Name, Freq = 0, Last = (DateTime?)null, IsPrefix = false });
        var fromItemsSub = _ctx.InvoiceItems
            .Where(i => !EF.Functions.Like(i.Product.Name, likePrefix) && EF.Functions.Like(i.Product.Name, likeContains))
            .GroupBy(i => i.Product.Name)
            .Select(g => new { Name = g.Key, Freq = g.Count(), Last = (DateTime?)g.Max(i => i.Invoice.Date), IsPrefix = false });
        var fromTermsSub = _ctx.SuggestionTerms
            .Where(t => !EF.Functions.Like(t.Term, likePrefix) && EF.Functions.Like(t.Term, likeContains))
            .Select(t => new { Name = t.Term, Freq = t.Frequency, Last = (DateTime?)t.LastUsedUtc, IsPrefix = false });

        var combined = await fromProductsPrefix
            .Concat(fromSuppliersPrefix)
            .Concat(fromItemsPrefix)
            .Concat(fromTermsPrefix)
            .Concat(fromProductsSub)
            .Concat(fromSuppliersSub)
            .Concat(fromItemsSub)
            .Concat(fromTermsSub)
            .ToListAsync();

        var distinct = combined
            .GroupBy(x => x.Name.ToUpper())
            .Select(g => g
                .OrderByDescending(x => x.IsPrefix)
                .ThenByDescending(x => x.Freq)
                .ThenByDescending(x => x.Last)
                .First())
            .OrderByDescending(x => x.IsPrefix)
            .ThenByDescending(x => x.Freq)
            .ThenByDescending(x => x.Last)
            .Take(10)
            .Select(x => x.Name)
            .ToList();

        return distinct;
    }
}
