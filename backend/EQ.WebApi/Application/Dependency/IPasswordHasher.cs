using EQ.WebApi.Domain.Common;

namespace EQ.WebApi.Application.Dependency;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, PasswordHash passwordHash);
}
