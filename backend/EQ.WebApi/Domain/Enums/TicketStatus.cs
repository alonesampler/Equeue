namespace EQ.WebApi.Domain.Enums;

public enum TicketStatus
{
    Waiting = 1,    // ждёт в очереди
    InService = 2,  // оператор начал обслуживание
    Completed = 3,  // обслужен
    NoShow = 4,     // вызвали, клиент не пришёл
    Cancelled = 5   // отменён до вызова
}
