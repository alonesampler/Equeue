using EQ.WebApi.Domain.Enums;

namespace EQ.WebApi.Application.Dtos;

public sealed record AuthenticatedUserDto(Guid Id, string Login, string FullName, UserRole Role);
