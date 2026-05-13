using Application.Abstractions;
using MediatR;

namespace Application.UseCase.FlightStatusTransitions;

public sealed class DeleteFlightStatusTransitionHandler(IUnitOfWork uow) : IRequestHandler<DeleteFlightStatusTransition>
{
    public async Task Handle(DeleteFlightStatusTransition request, CancellationToken ct)
    {
        var transition = await uow.FlightStatusTransitions.GetByIdAsync(request.Id, ct);

        if (transition is null)
            throw new Exception($"FlightStatusTransition with id {request.Id} not found.");

        await uow.FlightStatusTransitions.RemoveAsync(transition, ct);
        await uow.SaveChangesAsync(ct);
    }
}
