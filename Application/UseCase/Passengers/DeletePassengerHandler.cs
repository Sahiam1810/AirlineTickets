using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Passengers;

public sealed class DeletePassengerHandler(IUnitOfWork uow) : IRequestHandler<DeletePassenger>
{
    public async Task Handle(DeletePassenger request, CancellationToken ct)
    {
        var passenger = await uow.Passengers.GetByIdAsync(request.Id, ct);

        if (passenger is null)
            throw new Exception($"Passenger with id {request.Id} not found.");

        await uow.Passengers.RemoveAsync(passenger, ct);
        await uow.SaveChangesAsync(ct);
    }
}
