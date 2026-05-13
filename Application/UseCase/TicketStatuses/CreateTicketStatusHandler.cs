using Application.Abstractions;
using Domain.Entities.Tickets;
using MediatR;

namespace Application.UseCase.TicketStatuses;

public sealed class CreateTicketStatusHandler(IUnitOfWork uow) : IRequestHandler<CreateTicketStatus, int>
{
    public async Task<int> Handle(CreateTicketStatus request, CancellationToken ct)
    {
        var name = request.Name.Trim();

        if (await uow.TicketStatuses.ExistsAsync(name, null, ct))
        {
            throw new InvalidOperationException($"TicketStatus with name {name} already exists.");
        }

        var ticketStatus = new TicketStatus(name);

        await uow.TicketStatuses.AddAsync(ticketStatus, ct);
        await uow.SaveChangesAsync(ct);

        return ticketStatus.Id;
    }
}
