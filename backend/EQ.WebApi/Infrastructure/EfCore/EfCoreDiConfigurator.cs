using EQ.WebApi.Application.Dependency;
using EQ.WebApi.Domain.Repositories;
using EQ.WebApi.Infrastructure.EfCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EQ.WebApi.Infrastructure.EfCore;

internal static class EfCoreDiConfigurator
{
    internal static void ConnectDbContext(IServiceCollection services, IConfiguration configuration, string connectionString)
    {
        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseNpgsql(connectionString,
                x =>
                {
                    x.MigrationsHistoryTable("__EFMigrationsHistory");
                });
        });
    }

    internal static void AddRepositories(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IChapterRepository, ChapterRepository>();
        services.AddScoped<ITicketCounterRepository, TicketCounterRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWindowRepository, WindowRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
