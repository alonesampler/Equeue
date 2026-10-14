using EQ.WebApi.Domain.Entities;
using EQ.WebApi.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EQ.WebApi.Infrastructure.EfCore.Repositories;

// ============================================================
// Window  (интерфейс: IWindowRepository : IRepository<Window, int>)
// ============================================================
public class WindowRepository(DatabaseContext context)
    : Repository<Window, int>(context), IWindowRepository
{
    // Грузим только ОТКРЫТЫЕ сессии, а не всю историю окна
    public Task<Window?> GetWithSectionsAsync(int id, CancellationToken ct = default)
        => Set.Include(w => w.Sections.Where(s => s.EndedAt == null))
              .FirstOrDefaultAsync(w => w.Id == id, ct);

    public Task<Window?> GetByNameAsync(string name, CancellationToken ct = default)
        => Set.FirstOrDefaultAsync(window => window.Name == name, ct);

    public async Task<bool> HasReferencesAsync(int id, CancellationToken ct = default)
        => await Context.Tickets.AnyAsync(ticket => ticket.WindowId == id, ct)
            || await Context.WindowsSections.AnyAsync(section => section.WindowId == id, ct);
}