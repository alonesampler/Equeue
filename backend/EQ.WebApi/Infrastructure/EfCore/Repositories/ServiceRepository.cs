using EQ.WebApi.Domain.Entities;
using EQ.WebApi.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EQ.WebApi.Infrastructure.EfCore.Repositories;

// ============================================================
// Service
// ============================================================
public class ServiceRepository(DatabaseContext context)
    : Repository<Service, Guid>(context), IServiceRepository
{
    public async Task<IReadOnlyList<Service>> GetByChapterAsync(Guid chapterId, CancellationToken ct = default)
        => await Context.Chapters
            .Where(c => c.Id == chapterId)
            .SelectMany(c => c.Services)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<(Chapter Chapter, Service Service)>> GetAllWithChapterAsync(
        CancellationToken ct = default)
    {
        var chapters = await Context.Chapters
            .AsNoTracking()
            .Include(chapter => chapter.Services)
            .ToListAsync(ct);

        return chapters
            .SelectMany(chapter => chapter.Services.Select(service => (chapter, service)))
            .ToArray();
    }

    public Task<Guid?> GetChapterIdAsync(Guid serviceId, CancellationToken ct = default)
        => Context.Chapters
            .Where(chapter => chapter.Services.Any(service => service.Id == serviceId))
            .Select(chapter => (Guid?)chapter.Id)
            .FirstOrDefaultAsync(ct);

    public Task<bool> HasTicketsAsync(Guid serviceId, CancellationToken ct = default)
        => Context.Tickets.AnyAsync(ticket => ticket.ServiceId == serviceId, ct);

    public Task<bool> HasTicketCounterAsync(Guid serviceId, CancellationToken ct = default)
        => Context.TicketCounters.AnyAsync(counter => counter.ServiceId == serviceId, ct);
}
