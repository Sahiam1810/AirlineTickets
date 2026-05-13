using Application.Abstractions;
using MediatR;

namespace Application.UseCase.TicketStatuses;

public sealed class DeleteTicketStatusHandler(IUnitOfWork uow) : IRequestHandler<DeleteTicketStatus>
{
    public async Task Handle(DeleteTicketStatus request, CancellationToken ct)
    {
        var ticketStatus = await uow.TicketStatuses.GetByIdAsync(request.Id, ct);

        if (ticketStatus is null)
        {
            throw new KeyNotFoundException($"TicketStatus with id {request.Id} was not found.");
        }

        await uow.TicketStatuses.RemoveAsync(ticketStatus, ct);
        await uow.SaveChangesAsync(ct);
    }
}
