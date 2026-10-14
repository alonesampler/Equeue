using EQ.WebApi.Domain.Entities;

namespace EQ.WebApi.Domain.Repositories;

public interface ITicketCounterRepository : IRepository<TicketCounter, Guid>
{
    Task<TicketCounter?> GetByServiceAsync(Guid serviceId, CancellationToken ct = default);
}
