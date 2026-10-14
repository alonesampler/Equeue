using EQ.WebApi.Domain.Entities;
using EQ.WebApi.Domain.Enums;
using EQ.WebApi.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EQ.WebApi.Infrastructure.EfCore.Repositories;

// ============================================================
// Ticket
// ============================================================
public class TicketRepository(DatabaseContext context)
    : Repository<Ticket, Guid>(context), ITicketRepository
{
    public Task<Ticket?> GetNextWaitingAsync(Guid serviceId, CancellationToken ct = default)
        => Set.Where(t => t.ServiceId == serviceId && t.Status == TicketStatus.Waiting)
              .OrderBy(t => t.CreatedAt).ThenBy(t => t.Number)
              .FirstOrDefaultAsync(ct);

    // Дополнение к интерфейсу: оператор обычно работает с несколькими услугами
    public Task<Ticket?> GetNextWaitingAsync(IReadOnlyCollection<Guid> serviceIds, CancellationToken ct = default)
        => Set.Where(t => serviceIds.Contains(t.ServiceId) && t.Status == TicketStatus.Waiting)
              .OrderBy(t => t.CreatedAt).ThenBy(t => t.Number)
              .FirstOrDefaultAsync(ct);

    public async Task<IReadOnlyList<Ticket>> GetActiveByWindowAsync(int windowId, CancellationToken ct = default)
        => await Set
            .Where(t => t.WindowId == windowId && t.Status == TicketStatus.InService)
            .OrderBy(t => t.StartedAt)
            .ToListAsync(ct);
}
