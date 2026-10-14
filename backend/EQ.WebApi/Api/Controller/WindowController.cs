using System.ComponentModel.DataAnnotations;
using EQ.WebApi.Application.Dependency;
using EQ.WebApi.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EQ.WebApi.Api.Controller;

[ApiController]
[Route("api/windows")]
[Authorize(Roles = "Admin")]
public sealed class WindowController(IWindowService windowService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<WindowDto[]>> GetAll(CancellationToken ct)
    {
        var result = await windowService.GetAll(ct);
        return Ok(result.Value);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<WindowDto>> GetById(int id, CancellationToken ct)
    {
        var result = await windowService.GetById(id, ct);
        return result.IsFailed
            ? NotFound(new { message = result.Errors[0].Message })
            : Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create(WindowRequest request, CancellationToken ct)
    {
        var result = await windowService.Create(new WindowParams(request.Name, request.IsActive), ct);
        if (result.IsFailed)
            return result.Errors[0].Message.Contains("уже существует", StringComparison.Ordinal)
                ? Conflict(new { message = result.Errors[0].Message })
                : BadRequest(new { message = result.Errors[0].Message });

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, new { id = result.Value });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, WindowRequest request, CancellationToken ct)
    {
        var result = await windowService.Update(id, new WindowParams(request.Name, request.IsActive), ct);
        if (result.IsFailed)
        {
            var message = result.Errors[0].Message;
            return message == "Окно не найдено."
                ? NotFound(new { message })
                : message.Contains("уже существует", StringComparison.Ordinal)
                    ? Conflict(new { message })
                    : BadRequest(new { message });
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await windowService.Delete(id, ct);
        if (result.IsFailed)
        {
            var message = result.Errors[0].Message;
            return message == "Окно не найдено."
                ? NotFound(new { message })
                : Conflict(new { message });
        }

        return NoContent();
    }

    public sealed record WindowRequest(
        [param: Required, StringLength(100, MinimumLength = 1)] string Name,
        bool IsActive = true);
}
