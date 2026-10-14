using EQ.WebApi.Domain.Entities;

namespace EQ.WebApi.Domain.Repositories;

public interface IUserRepository : IRepository<User, Guid>
{
    Task<User?> GetByLoginAsync(string login, CancellationToken ct = default);
    Task<IReadOnlyList<User>> GetOperatorsByChapterAsync(Guid chapterId, CancellationToken ct = default);
}
