using Application.Abstractions;
using MediatR;

namespace Application.UseCase.FlightStates;

public sealed class DeleteFlightStateHandler(IUnitOfWork uow) : IRequestHandler<DeleteFlightState>
{
    public async Task Handle(DeleteFlightState request, CancellationToken ct)
    {
        var flightState = await uow.FlightStates.GetByIdAsync(request.Id, ct);

        if (flightState is null)
            throw new Exception($"FlightState with id {request.Id} not found.");

        await uow.FlightStates.RemoveAsync(flightState, ct);
        await uow.SaveChangesAsync(ct);
    }
}
