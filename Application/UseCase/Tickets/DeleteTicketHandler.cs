using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Tickets;

public sealed class DeleteTicketHandler(IUnitOfWork uow) : IRequestHandler<DeleteTicket>
{
    public async Task Handle(DeleteTicket request, CancellationToken ct)
    {
        var ticket = await uow.Tickets.GetByIdAsync(request.Id, ct);

        if (ticket is null)
        {
            throw new KeyNotFoundException($"Ticket with id {request.Id} was not found.");
        }

        await uow.Tickets.RemoveAsync(ticket, ct);
        await uow.SaveChangesAsync(ct);
    }
}
