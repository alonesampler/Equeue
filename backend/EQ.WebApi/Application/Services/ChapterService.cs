using EQ.WebApi.Application.Dependency;
using EQ.WebApi.Application.Dtos;
using EQ.WebApi.Application.Extensions;
using EQ.WebApi.Domain.Entities;
using FluentResults;

namespace EQ.WebApi.Application.Services;

public sealed class ChapterService(IUnitOfWork unitOfWork) : IChapterService
{
    public async Task<Result<ChapterDto[]>> GetAll(CancellationToken ct = default)
    {
        var chapters = await unitOfWork.Chapter.GetAllAsync(ct);
        var dtos = chapters.ToDto();
        return Result.Ok(dtos);
    }

    public async Task<Result<ChapterDto>> GetById(Guid id, CancellationToken ct = default)
    {
        var chapter = await unitOfWork.Chapter.GetByIdAsync(id, ct);
        return chapter is null
            ? Result.Fail<ChapterDto>("Раздел не найден.")
            : Result.Ok(chapter.ToDto());
    }

    public async Task<Result<Guid>> CreateAsync(ChapterParams @params, CancellationToken ct = default)
    {
        var validationError = Validate(@params);
        if (validationError is not null)
            return Result.Fail<Guid>(validationError);

        if (await unitOfWork.Chapter.GetByCodeAsync(@params.Code, ct) is not null)
            return Result.Fail<Guid>("Раздел с таким кодом уже существует.");

        var chapter = Chapter.CanCreate(
            Guid.NewGuid(),
            @params.Code,
            @params.Name,
            @params.Description,
            @params.IsActive);

        if (chapter.IsFailed)
            return Result.Fail<Guid>(chapter.Errors);

        await unitOfWork.Chapter.AddAsync(chapter.Value, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return Result.Ok(chapter.Value.Id);
    }

    public async Task<Result> Update(Guid id, ChapterParams @params, CancellationToken ct = default)
    {
        var chapter = await unitOfWork.Chapter.GetByIdAsync(id, ct);
        if (chapter is null)
            return Result.Fail("Раздел не найден.");

        var validationError = Validate(@params);
        if (validationError is not null)
            return Result.Fail(validationError);

        var existing = await unitOfWork.Chapter.GetByCodeAsync(@params.Code, ct);
        if (existing is not null && existing.Id != id)
            return Result.Fail("Раздел с таким кодом уже существует.");

        chapter.Update(@params.Code, @params.Name, @params.Description, @params.IsActive);
        await unitOfWork.SaveChangesAsync(ct);
        return Result.Ok();
    }

    public async Task<Result> Delete(Guid id, CancellationToken ct = default)
    {
        var chapter = await unitOfWork.Chapter.GetByIdAsync(id, ct);
        if (chapter is null)
            return Result.Fail("Раздел не найден.");

        if (await unitOfWork.Chapter.HasTicketsAsync(id, ct))
            return Result.Fail("Нельзя удалить раздел, к которому привязаны талоны.");

        if (chapter.Services.Count > 0)
            return Result.Fail("Сначала удалите или перенесите услуги из раздела.");

        unitOfWork.Chapter.Remove(chapter);
        await unitOfWork.SaveChangesAsync(ct);
        return Result.Ok();
    }

    private static string? Validate(ChapterParams parameters)
    {
        if (parameters.Code.Length is < 1 or > 10)
            return "Код раздела должен содержать от 1 до 10 символов.";
        if (parameters.Name.Length is < 1 or > 200)
            return "Название раздела должно содержать от 1 до 200 символов.";
        if (parameters.Description?.Length > 1000)
            return "Описание раздела не должно превышать 1000 символов.";

        return null;
    }
}
