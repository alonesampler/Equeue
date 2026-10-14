using EQ.WebApi.Domain.Common;
using EQ.WebApi.Domain.Enums;
using FluentResults;

namespace EQ.WebApi.Domain.Entities;

public class Ticket : Entity<Guid>
{
    private Ticket() { }

    private Ticket(Guid id, Guid chapterId, string chapterCode, Guid serviceId, int number, TicketStatus status, DateTime createdAt)
    {
        Id = id;
        ChapterId = chapterId;
        ChapterCode = chapterCode;
        ServiceId = serviceId;
        Number = number;
        Status = status;
        CreatedAt = createdAt;
    }

    public Guid ChapterId { get; private set; }
    public string ChapterCode { get; private set; }

    public Guid ServiceId { get; private set; }

    public int Number { get; private set; }
    public TicketStatus Status { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? StartedAt { get; private set; }
    public DateTime? FinishedAt { get; private set; }

    public Guid? UserId { get; private set; }
    public int? WindowId { get; private set; }

    public static Result<Ticket> CanCreate(Guid id, Guid chapterId, string chapterCode, Guid serviceId, int number, TicketStatus status, DateTime createdAt)
    {
        var ticket = new Ticket(id, chapterId, chapterCode, serviceId, number, status, createdAt);

        return Result.Ok(ticket);
    }
}
