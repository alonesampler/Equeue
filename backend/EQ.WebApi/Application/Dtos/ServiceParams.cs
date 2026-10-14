namespace EQ.WebApi.Application.Dtos;

public sealed record ServiceParams(
    Guid ChapterId,
    string Name,
    string? Description,
    bool IsActive);
