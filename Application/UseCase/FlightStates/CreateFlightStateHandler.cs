using Application.Abstractions;
using Domain.Entities.Flights;
using Domain.ValueObjects.FlightStates;
using MediatR;

namespace Application.UseCase.FlightStates;

public sealed class CreateFlightStateHandler : IRequestHandler<CreateFlightState, int>
{
    private readonly IUnitOfWork uow;

    public CreateFlightStateHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateFlightState req, CancellationToken ct)
    {
        var name = FlightStateName.Create(req.Name);

        if (await uow.FlightStates.ExistsByNameAsync(name, ct))
            throw new Exception($"FlightState with name {name.Value} already exists.");

        var flightState = new FlightState(req.Name);

        await uow.FlightStates.AddAsync(flightState, ct);
        await uow.SaveChangesAsync(ct);
        return flightState.Id;
    }
}
