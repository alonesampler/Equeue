namespace EQ.WebApi.Application.Dtos;

public sealed record ChapterParams(string Code, string Name, string? Description, bool IsActive = true);
