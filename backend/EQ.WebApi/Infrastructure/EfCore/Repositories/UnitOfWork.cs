using EQ.WebApi.Application.Dependency;
using EQ.WebApi.Domain.Repositories;

namespace EQ.WebApi.Infrastructure.EfCore.Repositories;

// ============================================================
// Unit of Work (интерфейс в Application)
// ============================================================


public class UnitOfWork(
    DatabaseContext context,
    IUserRepository userRepository,
    IServiceRepository serviceRepository,
    IChapterRepository chapterRepository,
    ITicketRepository ticketRepository,
    ITicketCounterRepository ticketCounterRepository,
    IWindowRepository windowRepository) : IUnitOfWork
{
    public IUserRepository User => userRepository;

    public IServiceRepository Service => serviceRepository;

    public IChapterRepository Chapter => chapterRepository;

    public ITicketRepository Ticket => ticketRepository;

    public ITicketCounterRepository TicketCounter => ticketCounterRepository;

    public IWindowRepository Window => windowRepository;

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => context.SaveChangesAsync(ct);
}
