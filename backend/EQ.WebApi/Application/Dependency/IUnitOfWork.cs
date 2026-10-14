using EQ.WebApi.Domain.Repositories;

namespace EQ.WebApi.Application.Dependency;

public interface IUnitOfWork
{
    IUserRepository User { get; }
    IServiceRepository Service { get; }
    IChapterRepository Chapter { get; }
    ITicketRepository Ticket { get; }
    ITicketCounterRepository TicketCounter { get; }
    IWindowRepository Window { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
