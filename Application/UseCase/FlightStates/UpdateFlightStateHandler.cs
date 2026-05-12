using Application.Abstractions;
using Domain.ValueObjects.FlightStates;
using MediatR;

namespace Application.UseCase.FlightStates;

public sealed class UpdateFlightStateHandler(IUnitOfWork uow) : IRequestHandler<UpdateFlightState>
{
    public async Task Handle(UpdateFlightState request, CancellationToken ct)
    {
        var flightState = await uow.FlightStates.GetByIdAsync(request.Id, ct);

        if (flightState is null)
            throw new Exception($"FlightState with id {request.Id} not found.");

        var name = FlightStateName.Create(request.Name);
        var sameName = string.Equals(flightState.Name.Value, name.Value, StringComparison.OrdinalIgnoreCase);

        if (!sameName && await uow.FlightStates.ExistsByNameAsync(name, ct))
            throw new Exception($"FlightState with name {name.Value} already exists.");

        flightState.Update(request.Name);

        await uow.FlightStates.UpdateAsync(flightState, ct);
        await uow.SaveChangesAsync(ct);
    }
}
