using EQ.WebApi.Domain.Common;
using FluentResults;

namespace EQ.WebApi.Domain.Entities;

public class Chapter : Entity<Guid>
{
    private Chapter() { }

    private Chapter(Guid id, string code, string name, string? description, bool isActive)
    {
        Id= id;
        Code = code;
        Name = name;
        Description = description;
        IsActive = isActive;
    }

    public string Code { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }

    private readonly List<Service> _services = new();
    public IReadOnlyCollection<Service> Services => _services;

    public static Result<Chapter> CanCreate(Guid id, string code, string name, string? description, bool isActive)
    {
        var chapter = new Chapter(id, code, name, description, isActive);

        return Result.Ok(chapter);
    }

    public void Update(string code, string name, string? description, bool isActive)
    {
        Code = code;
        Name = name;
        Description = description;
        IsActive = isActive;
    }

    public void AddService(Service service)
    {
        if (_services.Any(s => s.Id == service.Id))
            throw new DomainException("Услуга уже входит в раздел.");

        _services.Add(service);
    }

    public void RemoveService(Guid serviceId)
    {
        var service = _services.FirstOrDefault(s => s.Id == serviceId)
            ?? throw new DomainException("Услуги нет в этом разделе.");

        _services.Remove(service);
    }

    public bool ContainsService(Guid serviceId) => _services.Any(s => s.Id == serviceId);

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
