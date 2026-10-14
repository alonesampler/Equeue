using EQ.WebApi.Domain.Common;
using FluentResults;

namespace EQ.WebApi.Domain.Entities;

public class WindowSection : Entity<long>
{
    private WindowSection() { }

    public WindowSection(int windowId, Guid userId, DateTime startedAt)
    {
        WindowId = windowId;
        UserId = userId;
        StartedAt = startedAt;
    }

    public int WindowId { get; private set; }

    public Guid UserId { get; private set; }

    public DateTime StartedAt { get; private set; }

    public DateTime? EndedAt { get; private set; }

    public bool IsOpen => EndedAt is null;

    public static Result<WindowSection> CanCreate(int windowId, Guid userId, DateTime startedAt)
    {
        var section = new WindowSection(windowId, userId, startedAt);

        return Result.Ok(section);
    }
}
