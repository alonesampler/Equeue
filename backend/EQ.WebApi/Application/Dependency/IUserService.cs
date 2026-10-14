using EQ.WebApi.Application.Dtos;
using FluentResults;

namespace EQ.WebApi.Application.Dependency;

public interface IUserService
{
    Task<Result> Create(UserParams @params, CancellationToken ct = default);
    Task<Result<AuthenticatedUserDto>> Authenticate(string login, string password, CancellationToken ct = default);
    Task<Result<UserDto[]>> GetUserList(CancellationToken ct = default);
}