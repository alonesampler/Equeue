using EQ.WebApi.Application.Dependency;
using EQ.WebApi.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EQ.WebApi.Api.Controller;

[ApiController]
[Route("api/chapters")]
[Authorize(Roles = "Admin")]
public sealed class ChapterController(IChapterService chapterService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ChapterDto[]>> GetAll(CancellationToken ct)
    {
        var result = await chapterService.GetAll(ct);
        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ChapterDto>> GetById(Guid id, CancellationToken ct)
    {
        var result = await chapterService.GetById(id, ct);
        return result.IsFailed ? NotFound(new { message = result.Errors[0].Message }) : Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ChapterParams request, CancellationToken ct)
    {
        var result = await chapterService.CreateAsync(
            new ChapterParams(request.Code, request.Name, request.Description, request.IsActive),
            ct);

        if (result.IsFailed)
            return result.Errors[0].Message.Contains("уже существует", StringComparison.Ordinal)
                ? Conflict(new { message = result.Errors[0].Message })
                : BadRequest(new { message = result.Errors[0].Message });

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, new { id = result.Value });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, ChapterParams request, CancellationToken ct)
    {
        var result = await chapterService.Update(
            id,
            new ChapterParams(request.Code, request.Name, request.Description, request.IsActive),
            ct);

        if (result.IsFailed)
        {
            var message = result.Errors[0].Message;
            return message == "Раздел не найден."
                ? NotFound(new { message })
                : message.Contains("уже существует", StringComparison.Ordinal)
                    ? Conflict(new { message })
                    : BadRequest(new { message });
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await chapterService.Delete(id, ct);
        if (result.IsFailed)
            return result.Errors[0].Message == "Раздел не найден."
                ? NotFound(new { message = result.Errors[0].Message })
                : Conflict(new { message = result.Errors[0].Message });

        return NoContent();
    }
}
