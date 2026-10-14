using EQ.WebApi.Application.Dtos;
using FluentResults;

namespace EQ.WebApi.Application.Dependency;

public interface IServiceService
{
    Task<Result<ServiceDto[]>> GetAll(CancellationToken ct = default);
    Task<Result<ServiceDto>> GetById(Guid id, CancellationToken ct = default);
    Task<Result<Guid>> Create(ServiceParams parameters, CancellationToken ct = default);
    Task<Result> Update(Guid id, ServiceParams parameters, CancellationToken ct = default);
    Task<Result> Delete(Guid id, CancellationToken ct = default);
}
