using Entity.Base;

namespace Services;

/// <summary>
/// Базовый интерфейс репозиториев.
/// </summary>
public interface IRepository<T> where T : EntityBase
{
    public IQueryable<T> Items { get; }
    public Task<T?> GetAsync(int id, CancellationToken cancel = default);
    public Task<T> CreateAsync(T entity, CancellationToken cancel = default);
    public Task<T> EditAsync(T entity, CancellationToken cancel = default);
    public Task RemoveAsync(T entity, CancellationToken cancel = default);
}
