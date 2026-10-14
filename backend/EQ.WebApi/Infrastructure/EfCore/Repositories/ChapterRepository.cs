using EQ.WebApi.Domain.Entities;
using EQ.WebApi.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EQ.WebApi.Infrastructure.EfCore.Repositories;

// ============================================================
// Chapter: всегда грузим услуги, т.к. AddService/RemoveService работают с _services
// ============================================================
public class ChapterRepository(DatabaseContext context)
    : Repository<Chapter, Guid>(context), IChapterRepository
{
    public override Task<Chapter[]> GetAllAsync(CancellationToken ct = default)
        => Set.Include(c => c.Services).ToArrayAsync(ct);

    public override Task<Chapter?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => Set.Include(c => c.Services).FirstOrDefaultAsync(c => c.Id == id, ct);

    public Task<Chapter?> GetByCodeAsync(string code, CancellationToken ct = default)
        => Set.FirstOrDefaultAsync(c => c.Code == code, ct);

    public Task<bool> HasTicketsAsync(Guid chapterId, CancellationToken ct = default)
        => Context.Tickets.AnyAsync(ticket => ticket.ChapterId == chapterId, ct);
}
