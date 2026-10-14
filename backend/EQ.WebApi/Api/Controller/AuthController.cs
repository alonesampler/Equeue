using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EQ.WebApi.Application.Dependency;
using EQ.WebApi.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EQ.WebApi.Api.Controller;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IUserService userService, IConfiguration configuration) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request, CancellationToken ct)
    {
        var result = await userService.Authenticate(request.Login, request.Password, ct);
        if (result.IsFailed)
            return Unauthorized(new { message = result.Errors[0].Message });

        var user = result.Value;
        var issuer = configuration["Jwt:Issuer"] ?? "EQ.WebApi";
        var audience = configuration["Jwt:Audience"] ?? "EQ.WebApi.Client";
        var signingKey = configuration["Jwt:SigningKey"]
            ?? throw new InvalidOperationException("JWT signing key is not configured.");
        var expiresAt = DateTime.UtcNow.AddHours(1);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Login),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
            SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return Ok(new LoginResponse(
            new JwtSecurityTokenHandler().WriteToken(token),
            expiresAt,
            user));
    }

    [Authorize]
    [HttpGet("me")]
    public ActionResult<AuthUserResponse> Me()
    {
        var id = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        var login = User.FindFirstValue(JwtRegisteredClaimNames.UniqueName);
        var fullName = User.FindFirstValue(ClaimTypes.Name);
        var role = User.FindFirstValue(ClaimTypes.Role);

        if (!Guid.TryParse(id, out var userId) || login is null || fullName is null || role is null)
            return Unauthorized();

        return Ok(new AuthUserResponse(userId, login, fullName, role));
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
        => Ok(new { message = "Удалите access-токен на клиенте." });

    public sealed record LoginRequest(
        [param: Required] string Login,
        [param: Required] string Password);

    public sealed record LoginResponse(string AccessToken, DateTime ExpiresAt, AuthenticatedUserDto User);
    public sealed record AuthUserResponse(Guid Id, string Login, string FullName, string Role);
}
