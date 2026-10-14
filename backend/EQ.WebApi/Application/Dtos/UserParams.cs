using EQ.WebApi.Domain.Common;
using EQ.WebApi.Domain.Enums;

namespace EQ.WebApi.Application.Dtos;

public record UserParams(string Login, string Password, UserRole Role, FullName FullName);
