using Application.Abstractions;
using Domain.ValueObjects.FlightStatusTransitions;
using MediatR;

namespace Application.UseCase.FlightStatusTransitions;

public sealed class UpdateFlightStatusTransitionHandler(IUnitOfWork uow) : IRequestHandler<UpdateFlightStatusTransition>
{
    public async Task Handle(UpdateFlightStatusTransition request, CancellationToken ct)
    {
        var transition = await uow.FlightStatusTransitions.GetByIdAsync(request.Id, ct);

        if (transition is null)
            throw new Exception($"FlightStatusTransition with id {request.Id} not found.");

        Validate(request.FromStateId, request.ToStateId);

        await ValidateStates(request.FromStateId, request.ToStateId, ct);

        if (await uow.FlightStatusTransitions.ExistsAsync(request.FromStateId, request.ToStateId, request.Id, ct))
            throw new Exception("FlightStatusTransition already exists for this origin and destination state.");

        transition.Update(request.FromStateId, request.ToStateId);

        await uow.FlightStatusTransitions.UpdateAsync(transition, ct);
        await uow.SaveChangesAsync(ct);
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
