using EQ.WebApi.Domain.Entities;

namespace EQ.WebApi.Domain.Repositories;

public interface ITicketRepository : IRepository<Ticket, Guid>
{
    Task<Ticket?> GetNextWaitingAsync(Guid serviceId, CancellationToken ct = default);
    Task<IReadOnlyList<Ticket>> GetActiveByWindowAsync(int windowId, CancellationToken ct = default);
}
