using EQ.WebApi.Application.Dependency;
using EQ.WebApi.Application.Dtos;
using EQ.WebApi.Application.Extensions;
using EQ.WebApi.Domain.Entities;
using FluentResults;

namespace EQ.WebApi.Application.Services;

public sealed class ServiceService(IUnitOfWork unitOfWork) : IServiceService
{
    public async Task<Result<ServiceDto[]>> GetAll(CancellationToken ct = default)
    {
        var services = await unitOfWork.Service.GetAllAsync(ct);
        return Result.Ok(services.ToDto());
    }

    public async Task<Result<ServiceDto>> GetById(Guid id, CancellationToken ct = default)
    {
        var service = await unitOfWork.Service.GetByIdAsync(id, ct);
        if (service is null)
            return Result.Fail<ServiceDto>("Услуга не найдена.");

        var chapter = await unitOfWork.Chapter.GetByIdAsync(service.ChapterId, ct);
        if (chapter is null)
            return Result.Fail<ServiceDto>("Услуга не привязана к разделу.");

        var dto = service.ToDto();

        return Result.Ok(dto);
    }

    public async Task<Result<Guid>> Create(ServiceParams @params, CancellationToken ct = default)
    {
        var validationError = Validate(@params);
        if (validationError is not null)
            return Result.Fail<Guid>(validationError);

        var chapter = await unitOfWork.Chapter.GetByIdAsync(@params.ChapterId, ct);
        if (chapter is null)
            return Result.Fail<Guid>("Раздел не найден.");

        var created = Service.CanCreate(
            Guid.NewGuid(),
            @params.Name,
            @params.Description,
            @params.IsActive,
            @params.ChapterId);

        if (created.IsFailed)
            return Result.Fail<Guid>(created.Errors);

        await unitOfWork.Service.AddAsync(created.Value, ct);
        chapter.AddService(created.Value);
        await unitOfWork.SaveChangesAsync(ct);
        return Result.Ok(created.Value.Id);
    }

    public async Task<Result> Update(Guid id, ServiceParams parameters, CancellationToken ct = default)
    {
        var normalized = Normalize(parameters);
        var validationError = Validate(normalized);
        if (validationError is not null)
            return Result.Fail(validationError);

        var service = await unitOfWork.Service.GetByIdAsync(id, ct);
        if (service is null)
            return Result.Fail("Услуга не найдена.");

        var targetChapter = await unitOfWork.Chapter.GetByIdAsync(normalized.ChapterId, ct);
        if (targetChapter is null)
            return Result.Fail("Раздел не найден.");

        var currentChapterId = await unitOfWork.Service.GetChapterIdAsync(id, ct);
        if (currentChapterId != normalized.ChapterId)
        {
            if (currentChapterId.HasValue)
            {
                var currentChapter = await unitOfWork.Chapter.GetByIdAsync(currentChapterId.Value, ct);
                if (currentChapter is null)
                    return Result.Fail("Текущий раздел услуги не найден.");

                currentChapter.RemoveService(id);
            }

            targetChapter.AddService(service);
        }

        service.Update(normalized.Name, normalized.Description, normalized.IsActive);
        await unitOfWork.SaveChangesAsync(ct);
        return Result.Ok();
    }

    public async Task<Result> Delete(Guid id, CancellationToken ct = default)
    {
        var service = await unitOfWork.Service.GetByIdAsync(id, ct);
        if (service is null)
            return Result.Fail("Услуга не найдена.");

        if (await unitOfWork.Service.HasTicketsAsync(id, ct))
            return Result.Fail("Нельзя удалить услугу, к которой привязаны талоны.");

        if (await unitOfWork.Service.HasTicketCounterAsync(id, ct))
            return Result.Fail("Нельзя удалить услугу, для которой уже создан счётчик талонов.");

        var chapterId = await unitOfWork.Service.GetChapterIdAsync(id, ct);
        if (chapterId.HasValue)
        {
            var chapter = await unitOfWork.Chapter.GetByIdAsync(chapterId.Value, ct);
            chapter?.RemoveService(id);
        }

        unitOfWork.Service.Remove(service);
        await unitOfWork.SaveChangesAsync(ct);
        return Result.Ok();
    }

    private static ServiceParams Normalize(ServiceParams parameters)
        => parameters with
        {
            Name = parameters.Name.Trim(),
            Description = parameters.Description?.Trim()
        };

    private static string? Validate(ServiceParams parameters)
    {
        if (parameters.Name.Length is < 1 or > 200)
            return "Название услуги должно содержать от 1 до 200 символов.";
        if (parameters.Description?.Length > 1000)
            return "Описание услуги не должно превышать 1000 символов.";
        if (parameters.ChapterId == Guid.Empty)
            return "Необходимо указать GUID раздела.";

        return null;
    }
}
