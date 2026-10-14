using EQ.WebApi.Application.Dependency;
using EQ.WebApi.Domain.Common;

namespace EQ.WebApi.Infrastructure.PasswordHasher;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }

    public bool VerifyPassword(string password, PasswordHash passwordHash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash.Value);
    }
}
