using EQ.WebApi.Application.Dtos;
using FluentResults;

namespace EQ.WebApi.Application.Dependency;

public interface IWindowService
{
    Task<Result<WindowDto[]>> GetAll(CancellationToken ct = default);
    Task<Result<WindowDto>> GetById(int id, CancellationToken ct = default);
    Task<Result<int>> Create(WindowParams parameters, CancellationToken ct = default);
    Task<Result> Update(int id, WindowParams parameters, CancellationToken ct = default);
    Task<Result> Delete(int id, CancellationToken ct = default);
}
