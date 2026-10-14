using EQ.WebApi.Domain.Entities;

namespace EQ.WebApi.Domain.Repositories;

public interface IWindowRepository : IRepository<Window, int>
{
    Task<Window?> GetWithSectionsAsync(int id, CancellationToken ct = default);
    Task<Window?> GetByNameAsync(string name, CancellationToken ct = default);
    Task<bool> HasReferencesAsync(int id, CancellationToken ct = default);
}
