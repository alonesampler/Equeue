using EQ.WebApi.Application.Dependency;
using EQ.WebApi.Application.Dtos;
using EQ.WebApi.Application.Extensions;
using EQ.WebApi.Domain.Entities;
using FluentResults;

namespace EQ.WebApi.Application.Services;

public class UserService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher) : IUserService
{
    public async Task<Result> Create(UserParams @params, CancellationToken ct = default)
    {
        var existing = await unitOfWork.User.GetByLoginAsync(@params.Login, ct);
        if (existing is not null)
            return Result.Fail("Пользователь с таким логином уже существует");

        var passwordHash = passwordHasher.HashPassword(@params.Password);

        var created = User.CanCreate(
            Guid.NewGuid(),
            @params.Login,
            passwordHash,
            @params.Role,
            @params.FullName);

        if (created.IsFailed)
            return created.ToResult();

        await unitOfWork.User.AddAsync(created.Value, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result.Ok();
    }

    public async Task<Result<AuthenticatedUserDto>> Authenticate(
        string login,
        string password,
        CancellationToken ct = default)
    {
        var user = await unitOfWork.User.GetByLoginAsync(login.Trim(), ct);
        if (user is null || !passwordHasher.VerifyPassword(password, new(user.PasswordHash)))
            return Result.Fail<AuthenticatedUserDto>("Неверный логин или пароль");

        return Result.Ok(new AuthenticatedUserDto(
            user.Id,
            user.Login,
            user.FullName.GetFullName(),
            user.Role));
    }

    public async Task<Result<UserDto[]>> GetUserList(CancellationToken ct = default)
    {
        var users = await unitOfWork.User.GetAllAsync();
        var dtos = users.ToDto();

        return Result.Ok(dtos);
    }
}
