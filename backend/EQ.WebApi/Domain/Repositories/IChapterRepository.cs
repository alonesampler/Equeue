using EQ.WebApi.Domain.Entities;

namespace EQ.WebApi.Domain.Repositories;

public interface IChapterRepository : IRepository<Chapter, Guid>
{
    Task<Chapter?> GetByCodeAsync(string code, CancellationToken ct = default);
    Task<bool> HasTicketsAsync(Guid chapterId, CancellationToken ct = default);
}
