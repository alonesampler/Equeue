using EQ.WebApi.Domain.Entities;

namespace EQ.WebApi.Domain.Repositories;

public interface IServiceRepository : IRepository<Service, Guid>
{
    Task<IReadOnlyList<Service>> GetByChapterAsync(Guid chapterId, CancellationToken ct = default);
    Task<IReadOnlyList<(Chapter Chapter, Service Service)>> GetAllWithChapterAsync(CancellationToken ct = default);
    Task<Guid?> GetChapterIdAsync(Guid serviceId, CancellationToken ct = default);
    Task<bool> HasTicketsAsync(Guid serviceId, CancellationToken ct = default);
    Task<bool> HasTicketCounterAsync(Guid serviceId, CancellationToken ct = default);
}
