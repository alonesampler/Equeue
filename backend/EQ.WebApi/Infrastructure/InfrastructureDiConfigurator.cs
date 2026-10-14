using EQ.WebApi.Infrastructure.EfCore;
using EQ.WebApi.Infrastructure.PasswordHasher;

namespace EQ.WebApi.Infrastructure;

public static class InfrastructureDiConfiguration
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("ConnectionStrings");
        var postgres = section["PostgreSql"];

        PasswordHasherDiConfigurator.ConfigurePasswordHasher(services, configuration);
        EfCoreDiConfigurator.ConnectDbContext(services, configuration, postgres);
        EfCoreDiConfigurator.AddRepositories(services, configuration);
    }
}
