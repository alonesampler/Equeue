using EQ.WebApi.Application.Dtos;
using EQ.WebApi.Domain.Entities;

namespace EQ.WebApi.Application.Extensions;

public static class UserMappingExtensions
{
    public static UserDto ToDto(this User user)
        => new(
            user.FullName.GetFullName(),
            Role: user.Role);   

    public static UserDto[] ToDto(this IEnumerable<User> users)
        => users.Select(ToDto).ToArray();
}

public static class ChapterMappingExtensions
{
    public static ChapterDto ToDto(this Chapter chapter)
        => new(
            chapter.Id,
            chapter.Code,
            chapter.Name,
            chapter.Description,
            chapter.IsActive,
            chapter.Services.Select(service => service.Id).ToArray());

    public static ChapterDto[] ToDto(this IEnumerable<Chapter> chapters)
        => chapters.Select(ToDto).ToArray();
}

public static class ServiceMappingExtensions
{
    public static ServiceDto ToDto(this Service service)
        => new(service.Id, service.Name, service.Description, service.IsActive, service.ChapterId);

    public static ServiceDto[] ToDto(this IEnumerable<Service> services)
        => services.Select(ToDto).ToArray();
}