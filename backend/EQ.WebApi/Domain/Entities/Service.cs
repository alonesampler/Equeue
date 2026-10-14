using EQ.WebApi.Domain.Common;
using FluentResults;

namespace EQ.WebApi.Domain.Entities;

public class Service: Entity<Guid>
{
    private Service() { }

    private Service(Guid id, string name, string? description, bool isActive, Guid chapterId)
    {
        Id = id;
        Name = name;
        Description = description;
        IsActive = isActive;
        ChapterId = chapterId;
    }

    public string Name { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }

    public Guid ChapterId { get; private set; }

    public static Result<Service> CanCreate(Guid id, string name, string? description, bool isActive, Guid chapterId)
    {
        var service = new Service(id, name, description, isActive, chapterId);

        return Result.Ok(service);
    }

    public void Update(string name, string? description, bool isActive)
    {
        Name = name;
        Description = description;
        IsActive = isActive;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
