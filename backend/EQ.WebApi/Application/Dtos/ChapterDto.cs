namespace EQ.WebApi.Application.Dtos;

public sealed record ChapterDto(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    bool IsActive,
    Guid[] ServiceIds);
