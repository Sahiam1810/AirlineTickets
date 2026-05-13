using Application.Abstractions;
using MediatR;

namespace Application.UseCase.FlightAssignments;

public sealed class DeleteFlightAssignmentHandler(IUnitOfWork uow) : IRequestHandler<DeleteFlightAssignment>
{
    public async Task Handle(DeleteFlightAssignment request, CancellationToken ct)
    {
        var flightAssignment = await uow.FlightAssignments.GetByIdAsync(request.Id, ct);

        if (flightAssignment is null)
            throw new Exception($"FlightAssignment with id {request.Id} not found.");

        await uow.FlightAssignments.RemoveAsync(flightAssignment, ct);
        await uow.SaveChangesAsync(ct);
    }
}
