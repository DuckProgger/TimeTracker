using Database;
using Entity.Base;
using Microsoft.EntityFrameworkCore;

namespace Services;

/// <summary>
/// Базовый класс репозитория для сущности.
/// </summary>
public class DbRepository<T> : IRepository<T> where T : EntityBase
{
    private readonly ApplicationContext dbContext;
    private readonly DbSet<T> dbSet;
    public DbRepository(ApplicationContext dbContext)
    {
        this.dbContext = dbContext;
        dbSet = dbContext.Set<T>();
    }

    public virtual IQueryable<T> Items => dbSet;

    public async Task<T?> GetAsync(int id, CancellationToken cancel = default) => await Items.
        SingleOrDefaultAsync(item => item.Id == id, cancel)
        .ConfigureAwait(false);

    public async Task<T> CreateAsync(T entity, CancellationToken cancel = default)
    {
        dbContext.Set<T>().Add(entity);
        await SaveChangesAsync(cancel);
        return entity;
    }

    public async Task<T> EditAsync(T entity, CancellationToken cancel = default)
    {
        dbContext.Set<T>().Update(entity);
        await SaveChangesAsync(cancel);
        return entity;
    }

    public async Task RemoveAsync(T entity, CancellationToken cancel = default)
    {
        dbContext.Set<T>().Remove(entity);
        await SaveChangesAsync(cancel);
    }

    private async Task SaveChangesAsync(CancellationToken cancel) => await dbContext.SaveChangesAsync(cancel).ConfigureAwait(false);
}
