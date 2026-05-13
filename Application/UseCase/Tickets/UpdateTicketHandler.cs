using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Tickets;

public sealed class UpdateTicketHandler(IUnitOfWork uow) : IRequestHandler<UpdateTicket>
{
    public async Task Handle(UpdateTicket request, CancellationToken ct)
    {
        var ticket = await uow.Tickets.GetByIdAsync(request.Id, ct);

        if (ticket is null)
        {
            throw new KeyNotFoundException($"Ticket with id {request.Id} was not found.");
        }

        var ticketCode = request.TicketCode.Trim();

        if (await uow.ReservationPassengers.GetByIdAsync(request.ReservationPassengerId, ct) is null)
        {
            throw new KeyNotFoundException($"ReservationPassenger with id {request.ReservationPassengerId} was not found.");
        }

        if (await uow.TicketStatuses.GetByIdAsync(request.TicketStatusId, ct) is null)
        {
            throw new KeyNotFoundException($"TicketStatus with id {request.TicketStatusId} was not found.");
        }

        if (await uow.Tickets.ExistsAsync(ticketCode, request.Id, ct))
        {
            throw new InvalidOperationException($"Ticket with code {ticketCode} already exists.");
        }

        if (await uow.Tickets.ExistsByReservationPassengerAsync(request.ReservationPassengerId, request.Id, ct))
        {
            throw new InvalidOperationException("The reservation passenger already has a ticket.");
        }

        ticket.Update(
            request.ReservationPassengerId,
            ticketCode,
            request.IssuedAt,
            request.TicketStatusId);

        await uow.Tickets.UpdateAsync(ticket, ct);
        await uow.SaveChangesAsync(ct);
    }
}
