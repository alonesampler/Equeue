using EQ.WebApi.Application.Dependency;
using EQ.WebApi.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EQ.WebApi.Api.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UserController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> RegisterUser(
        [FromServices] IUserService userService,
        UserParams @params,
        CancellationToken ct = default)
    {
        var result = await userService.Create(@params, ct);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<UserDto[]>> GetAllUsers(
        [FromServices] IUserService userService,
        CancellationToken ct = default)
    {
        var result = await userService.GetUserList(ct);

        return Ok(result.Value);
    }
}
