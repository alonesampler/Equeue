using EQ.WebApi.Application.Dependency;
using EQ.WebApi.Application.Services;

namespace EQ.WebApi.Application;

public static class ApplicationDiConfigurator
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IChapterService, ChapterService>();
        services.AddScoped<IServiceService, ServiceService>();
        services.AddScoped<IWindowService, WindowService>();
    }
}
