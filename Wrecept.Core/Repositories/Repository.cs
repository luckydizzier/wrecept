using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Data;

namespace Wrecept.Core.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

    public async Task<T> GetByIdAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
        {
            throw new InvalidOperationException($"Entity of type {typeof(T).Name} with id {id} not found.");
        }
        return entity;
    }

    public async Task AddAsync(T entity)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateAsync(T entity)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteAsync(T entity)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
