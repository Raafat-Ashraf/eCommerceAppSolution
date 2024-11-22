using eCommerceApp.Domain.Interfaces;
using eCommerceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.Infrastructure.Repositories;

public class GenericRepository<TEntity>(ApplicationDbContext context)
    : IGenericRepository<TEntity> where TEntity : class
{
    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await context.Set<TEntity>().FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await context.Set<TEntity>().AsNoTracking().ToListAsync();
    }

    public async Task<int> AddAsync(TEntity entity)
    {
        await context.Set<TEntity>().AddAsync(entity);
        return await context.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(TEntity entity)
    {
        context.Set<TEntity>().Update(entity);
        return await context.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(TEntity entity)
    {
        context.Set<TEntity>().Remove(entity);
        return await context.SaveChangesAsync();
    }
}
