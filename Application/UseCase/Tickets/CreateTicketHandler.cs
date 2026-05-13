using Application.Abstractions;
using Domain.Entities.Tickets;
using MediatR;

namespace Application.UseCase.Tickets;

public sealed class CreateTicketHandler(IUnitOfWork uow) : IRequestHandler<CreateTicket, int>
{
    public async Task<int> Handle(CreateTicket request, CancellationToken ct)
    {
        var ticketCode = request.TicketCode.Trim();

        if (await uow.ReservationPassengers.GetByIdAsync(request.ReservationPassengerId, ct) is null)
        {
            throw new KeyNotFoundException($"ReservationPassenger with id {request.ReservationPassengerId} was not found.");
        }

        if (await uow.TicketStatuses.GetByIdAsync(request.TicketStatusId, ct) is null)
        {
            throw new KeyNotFoundException($"TicketStatus with id {request.TicketStatusId} was not found.");
        }

        if (await uow.Tickets.ExistsAsync(ticketCode, null, ct))
        {
            throw new InvalidOperationException($"Ticket with code {ticketCode} already exists.");
        }

        if (await uow.Tickets.ExistsByReservationPassengerAsync(request.ReservationPassengerId, null, ct))
        {
            throw new InvalidOperationException("The reservation passenger already has a ticket.");
        }

        var ticket = new Ticket(
            request.ReservationPassengerId,
            ticketCode,
            request.IssuedAt,
            request.TicketStatusId);

        await uow.Tickets.AddAsync(ticket, ct);
        await uow.SaveChangesAsync(ct);

        return ticket.Id;
    }
}
