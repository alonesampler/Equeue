using EQ.WebApi.Domain.Entities;
using EQ.WebApi.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EQ.WebApi.Infrastructure.EfCore.Repositories;

// ============================================================
// TicketCounter
// ============================================================
public class TicketCounterRepository(DatabaseContext context)
    : Repository<TicketCounter, Guid>(context), ITicketCounterRepository
{
    public Task<TicketCounter?> GetByServiceAsync(Guid serviceId, CancellationToken ct = default)
        => Set.FirstOrDefaultAsync(c => c.ServiceId == serviceId, ct);
}
