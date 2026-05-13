using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Flights;

public sealed class ChangeFlightStateHandler(IUnitOfWork uow) : IRequestHandler<ChangeFlightState>
{
    public async Task Handle(ChangeFlightState request, CancellationToken ct)
    {
        var flight = await uow.Flights.GetByIdAsync(request.Id, ct);

        if (flight is null)
            throw new Exception($"Flight with id {request.Id} not found.");

        if (await uow.FlightStates.GetByIdAsync(request.FlightStateId, ct) is null)
            throw new Exception($"FlightState with id {request.FlightStateId} not found.");

        flight.ChangeState(request.FlightStateId);

        await uow.Flights.UpdateAsync(flight, ct);
        await uow.SaveChangesAsync(ct);
    }
}
