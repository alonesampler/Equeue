using EQ.WebApi.Domain.Entities;
using EQ.WebApi.Domain.Enums;
using EQ.WebApi.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EQ.WebApi.Infrastructure.EfCore.Repositories;

// ============================================================
// User: грузим разделы, т.к. нужен User.CanServe(...)
// ============================================================
public class UserRepository(DatabaseContext context)
    : Repository<User, Guid>(context), IUserRepository
{
    public override Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => Set.Include(u => u.Chapters).FirstOrDefaultAsync(u => u.Id == id, ct);

    public Task<User?> GetByLoginAsync(string login, CancellationToken ct = default)
        => Set.Include(u => u.Chapters).FirstOrDefaultAsync(u => u.Login == login, ct);

    public async Task<IReadOnlyList<User>> GetOperatorsByChapterAsync(Guid chapterId, CancellationToken ct = default)
        => await Set
            .Where(u => u.Role == UserRole.Operator && u.Chapters.Any(c => c.Id == chapterId))
            .ToListAsync(ct);
}
