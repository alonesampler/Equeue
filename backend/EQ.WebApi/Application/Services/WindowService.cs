using EQ.WebApi.Application.Dependency;
using EQ.WebApi.Application.Dtos;
using EQ.WebApi.Domain.Entities;
using FluentResults;

namespace EQ.WebApi.Application.Services;

public sealed class WindowService(IUnitOfWork unitOfWork) : IWindowService
{
    public async Task<Result<WindowDto[]>> GetAll(CancellationToken ct = default)
    {
        var windows = await unitOfWork.Window.GetAllAsync(ct);
        return Result.Ok(windows
            .OrderBy(window => window.Id)
            .Select(Map)
            .ToArray());
    }

    public async Task<Result<WindowDto>> GetById(int id, CancellationToken ct = default)
    {
        var window = await unitOfWork.Window.GetByIdAsync(id, ct);
        return window is null
            ? Result.Fail<WindowDto>("Окно не найдено.")
            : Result.Ok(Map(window));
    }

    public async Task<Result<int>> Create(WindowParams parameters, CancellationToken ct = default)
    {
        var normalized = Normalize(parameters);
        var validationError = Validate(normalized);
        if (validationError is not null)
            return Result.Fail<int>(validationError);

        if (await unitOfWork.Window.GetByNameAsync(normalized.Name, ct) is not null)
            return Result.Fail<int>("Окно с таким названием уже существует.");

        var created = Window.CanCreate(normalized.Name, normalized.IsActive);
        if (created.IsFailed)
            return Result.Fail<int>(created.Errors);

        await unitOfWork.Window.AddAsync(created.Value, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return Result.Ok(created.Value.Id);
    }

    public async Task<Result> Update(int id, WindowParams parameters, CancellationToken ct = default)
    {
        var window = await unitOfWork.Window.GetByIdAsync(id, ct);
        if (window is null)
            return Result.Fail("Окно не найдено.");

        var normalized = Normalize(parameters);
        var validationError = Validate(normalized);
        if (validationError is not null)
            return Result.Fail(validationError);

        var existing = await unitOfWork.Window.GetByNameAsync(normalized.Name, ct);
        if (existing is not null && existing.Id != id)
            return Result.Fail("Окно с таким названием уже существует.");

        window.Update(normalized.Name, normalized.IsActive);
        await unitOfWork.SaveChangesAsync(ct);
        return Result.Ok();
    }

    public async Task<Result> Delete(int id, CancellationToken ct = default)
    {
        var window = await unitOfWork.Window.GetByIdAsync(id, ct);
        if (window is null)
            return Result.Fail("Окно не найдено.");

        if (await unitOfWork.Window.HasReferencesAsync(id, ct))
            return Result.Fail("Нельзя удалить окно, которое использовалось в талонах или сменах. Деактивируйте его.");

        unitOfWork.Window.Remove(window);
        await unitOfWork.SaveChangesAsync(ct);
        return Result.Ok();
    }

    private static WindowDto Map(Window window)
        => new(window.Id, window.Name, window.IsActive);

    private static WindowParams Normalize(WindowParams parameters)
        => parameters with { Name = parameters.Name.Trim() };

    private static string? Validate(WindowParams parameters)
        => parameters.Name.Length is < 1 or > 100
            ? "Название окна должно содержать от 1 до 100 символов."
            : null;
}
