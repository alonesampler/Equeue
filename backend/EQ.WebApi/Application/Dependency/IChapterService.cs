using EQ.WebApi.Application.Dtos;
using FluentResults;

namespace EQ.WebApi.Application.Dependency;

public interface IChapterService
{
    Task<Result<ChapterDto[]>> GetAll(CancellationToken ct = default);
    Task<Result<ChapterDto>> GetById(Guid id, CancellationToken ct = default);
    Task<Result<Guid>> CreateAsync(ChapterParams parameters, CancellationToken ct = default);
    Task<Result> Update(Guid id, ChapterParams parameters, CancellationToken ct = default);
    Task<Result> Delete(Guid id, CancellationToken ct = default);
}
