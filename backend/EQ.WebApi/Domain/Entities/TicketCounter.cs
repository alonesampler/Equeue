using EQ.WebApi.Domain.Common;
using FluentResults;

namespace EQ.WebApi.Domain.Entities;

public class TicketCounter : Entity<Guid>
{
    private TicketCounter() { }
    private TicketCounter(Guid id, Guid serviceId, int lastNumber)
    {
        Id = id;
        ServiceId = serviceId;
        LastNumber = lastNumber;
    }

    public Guid ServiceId { get; private set; }
    public int LastNumber { get; private set; }

    public static Result<TicketCounter> CanCreate(Guid id, Guid serviceId, int lastNumber)
    {
        var ticketCounter = new TicketCounter(id, serviceId, lastNumber);

        return Result.Ok(ticketCounter);
    }

    public int Next() => ++LastNumber;
}
