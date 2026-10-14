using EQ.WebApi.Domain.Common;
using EQ.WebApi.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EQ.WebApi.Infrastructure.EfCore.Repositories;
// ============================================================
// Базовый репозиторий
// ============================================================
public class Repository<TEntity, TId>(DatabaseContext context) : IRepository<TEntity, TId>
    where TEntity : Entity<TId>
{
    protected DatabaseContext Context { get; } = context;
    protected DbSet<TEntity> Set => Context.Set<TEntity>();

    public virtual Task<TEntity[]> GetAllAsync(CancellationToken ct = default)
        => Set.ToArrayAsync(ct);

    public virtual async Task<TEntity?> GetByIdAsync(TId id, CancellationToken ct = default)
        => await Set.FindAsync(new object?[] { id }, ct);

    public virtual async Task AddAsync(TEntity entity, CancellationToken ct = default)
        => await Set.AddAsync(entity, ct);

    public virtual void Update(TEntity entity)
    {
        // Загруженные из контекста сущности EF отслеживает сам.
        // Update нужен только для "отвязанных" (detached).
        if (Context.Entry(entity).State == EntityState.Detached)
            Set.Update(entity);
    }

    public virtual void Remove(TEntity entity) => Set.Remove(entity);
}