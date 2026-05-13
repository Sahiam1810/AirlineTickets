using MediatR;

namespace Application.UseCase.Tickets;

public sealed record UpdateTicket(
    int Id,
    int ReservationPassengerId,
    string TicketCode,
    DateTime IssuedAt,
    int TicketStatusId) : IRequest;
