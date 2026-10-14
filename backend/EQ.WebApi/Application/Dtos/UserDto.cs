using EQ.WebApi.Domain.Enums;

namespace EQ.WebApi.Application.Dtos;

public sealed record UserDto(string FullName, UserRole Role);
