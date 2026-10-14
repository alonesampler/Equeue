using EQ.WebApi.Domain.Common;

namespace EQ.WebApi.Domain.Repositories;

public interface IRepository<TEntity, TId>
    where TEntity : Entity<TId>
{
    Task<TEntity[]> GetAllAsync(CancellationToken ct = default);
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken ct = default);
    Task AddAsync(TEntity entity, CancellationToken ct = default);
    void Update(TEntity entity);
    void Remove(TEntity entity);
}