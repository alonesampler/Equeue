using System.ComponentModel.DataAnnotations;
using EQ.WebApi.Application.Dependency;
using EQ.WebApi.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EQ.WebApi.Api.Controller;

[ApiController]
[Route("api/services")]
[Authorize(Roles = "Admin")]
public sealed class ServiceController(IServiceService serviceService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ServiceDto[]>> GetAll(CancellationToken ct)
    {
        var result = await serviceService.GetAll(ct);
        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ServiceDto>> GetById(Guid id, CancellationToken ct)
    {
        var result = await serviceService.GetById(id, ct);
        return result.IsFailed
            ? NotFound(new { message = result.Errors[0].Message })
            : Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(ServiceParams request, CancellationToken ct)
    {
        var result = await serviceService.Create(request, ct);

        if (result.IsFailed)
            return result.Errors[0].Message == "Раздел не найден."
                ? NotFound(new { message = result.Errors[0].Message })
                : BadRequest(new { message = result.Errors[0].Message });

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, new { id = result.Value });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, ServiceParams request, CancellationToken ct)
    {
        var result = await serviceService.Update(id, request, ct);

        if (result.IsFailed)
        {
            var message = result.Errors[0].Message;
            return message == "Услуга не найдена."
                ? NotFound(new { message })
                : message == "Раздел не найден."
                    ? NotFound(new { message })
                    : BadRequest(new { message });
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await serviceService.Delete(id, ct);
        if (result.IsFailed)
        {
            var message = result.Errors[0].Message;
            return message == "Услуга не найдена."
                ? NotFound(new { message })
                : Conflict(new { message });
        }

        return NoContent();
    }

    public sealed record ServiceRequest(
        [param: Required, StringLength(200, MinimumLength = 1)] string Name,
        [param: StringLength(1000)] string? Description,
        [param: Required] Guid ChapterId,
        bool IsActive = true);
}
