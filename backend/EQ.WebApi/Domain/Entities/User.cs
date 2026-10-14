using EQ.WebApi.Domain.Common;
using EQ.WebApi.Domain.Enums;
using FluentResults;

namespace EQ.WebApi.Domain.Entities;

public class User : Entity<Guid>
{
    private User() { }

    private User(Guid id, string login, string passwordHash, UserRole role, FullName fullName)
    {
        Id = id;
        Login = login;
        PasswordHash = passwordHash;
        Role = role;
        FullName = fullName;
    }

    public string Login { get; private set; }
    public string PasswordHash { get; private set; }
    public UserRole Role { get; private set; }
    public FullName FullName { get; private set; }

    private readonly List<Chapter> _chapters = new();
    public IReadOnlyCollection<Chapter> Chapters => _chapters;

    public static Result<User> CanCreate(Guid id, string login, string passwordHash, UserRole role, FullName fullName)
    {
        var user = new User(id, login, passwordHash, role, fullName);
        
        return Result.Ok(user);
    }
}
