using Application.Abstractions;
using Domain.Entities.Flights;
using Domain.ValueObjects.FlightStatusTransitions;
using MediatR;

namespace Application.UseCase.FlightStatusTransitions;

public sealed class CreateFlightStatusTransitionHandler : IRequestHandler<CreateFlightStatusTransition, int>
{
    private readonly IUnitOfWork uow;

    public CreateFlightStatusTransitionHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateFlightStatusTransition req, CancellationToken ct)
    {
        Validate(req.FromStateId, req.ToStateId);

        await ValidateStates(req.FromStateId, req.ToStateId, ct);

        if (await uow.FlightStatusTransitions.ExistsAsync(req.FromStateId, req.ToStateId, null, ct))
            throw new Exception("FlightStatusTransition already exists for this origin and destination state.");

        var transition = new FlightStatusTransition(req.FromStateId, req.ToStateId);

        await uow.FlightStatusTransitions.AddAsync(transition, ct);
        await uow.SaveChangesAsync(ct);
        return transition.Id;
    }

    private async Task ValidateStates(int fromStateId, int toStateId, CancellationToken ct)
    {
        if (await uow.FlightStates.GetByIdAsync(fromStateId, ct) is null)
            throw new Exception($"FlightState with id {fromStateId} not found.");

        if (await uow.FlightStates.GetByIdAsync(toStateId, ct) is null)
            throw new Exception($"FlightState with id {toStateId} not found.");
    }

    private static void Validate(int fromStateId, int toStateId)
    {
        _ = StateId.Create(fromStateId);
        _ = StateId.Create(toStateId);

        if (fromStateId == toStateId)
            throw new Exception("FromStateId cannot be equal to ToStateId.");
    }
}
