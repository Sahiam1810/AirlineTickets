using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Passengers;

public sealed class UpdatePassengerHandler(IUnitOfWork uow) : IRequestHandler<UpdatePassenger>
{
    public async Task Handle(UpdatePassenger request, CancellationToken ct)
    {
        var passenger = await uow.Passengers.GetByIdAsync(request.Id, ct);

        if (passenger is null)
            throw new Exception($"Passenger with id {request.Id} not found.");

        if (await uow.PassengerTypes.GetByIdAsync(request.PassengerTypeId, ct) is null)
            throw new Exception($"PassengerType with id {request.PassengerTypeId} not found.");

        passenger.Update(request.PassengerTypeId);

        await uow.Passengers.UpdateAsync(passenger, ct);
        await uow.SaveChangesAsync(ct);
    }
}
