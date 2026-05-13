using Application.Abstractions;
using MediatR;

namespace Application.UseCase.TicketStatuses;

public sealed class UpdateTicketStatusHandler(IUnitOfWork uow) : IRequestHandler<UpdateTicketStatus>
{
    public async Task Handle(UpdateTicketStatus request, CancellationToken ct)
    {
        var ticketStatus = await uow.TicketStatuses.GetByIdAsync(request.Id, ct);

        if (ticketStatus is null)
        {
            throw new KeyNotFoundException($"TicketStatus with id {request.Id} was not found.");
        }

        var name = request.Name.Trim();
        var sameName = string.Equals(ticketStatus.Name, name, StringComparison.OrdinalIgnoreCase);

        if (!sameName && await uow.TicketStatuses.ExistsAsync(name, request.Id, ct))
        {
            throw new InvalidOperationException($"TicketStatus with name {name} already exists.");
        }

        ticketStatus.Update(name);

        await uow.TicketStatuses.UpdateAsync(ticketStatus, ct);
        await uow.SaveChangesAsync(ct);
    }
}
