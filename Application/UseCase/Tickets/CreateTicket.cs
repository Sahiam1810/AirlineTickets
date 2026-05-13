using MediatR;

namespace Application.UseCase.Tickets;

public sealed record CreateTicket(
    int ReservationPassengerId,
    string TicketCode,
    DateTime IssuedAt,
    int TicketStatusId) : IRequest<int>;
