using EQ.WebApi.Domain.Common;
using FluentResults;

namespace EQ.WebApi.Domain.Entities;

public class Window : Entity<int>
{
    private Window() { }

    private Window(string name, bool isActive = true)
    {
        Name = name;
        IsActive = isActive;
    }

    public string Name { get; private set; }
    public bool IsActive { get; private set; }

    private readonly List<WindowSection> _sections = new();
    public IReadOnlyCollection<WindowSection> Sections => _sections;

    public static Result<Window> CanCreate(string name, bool isActive)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Fail("Название не может быть пустым");

        var window = new Window(name, isActive);

        return Result.Ok(window);
    }

    public void Update(string name, bool isActive)
    {
        Name = name;
        IsActive = isActive;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
