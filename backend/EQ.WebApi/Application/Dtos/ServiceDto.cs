namespace EQ.WebApi.Application.Dtos;

public sealed record ServiceDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    Guid ChapterId);
