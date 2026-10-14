using EQ.WebApi.Application.Dependency;

namespace EQ.WebApi.Infrastructure.PasswordHasher;

internal static class PasswordHasherDiConfigurator
{
    internal static void ConfigurePasswordHasher(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
    }
}
