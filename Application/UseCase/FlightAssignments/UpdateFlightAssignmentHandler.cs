using Application.Abstractions;
using MediatR;

namespace Application.UseCase.FlightAssignments;

public sealed class UpdateFlightAssignmentHandler(IUnitOfWork uow) : IRequestHandler<UpdateFlightAssignment>
{
    public async Task Handle(UpdateFlightAssignment request, CancellationToken ct)
    {
        var flightAssignment = await uow.FlightAssignments.GetByIdAsync(request.Id, ct);

        if (flightAssignment is null)
            throw new Exception($"FlightAssignment with id {request.Id} not found.");

        await ValidateReferences(request.FlightId, request.StaffId, request.FlightRoleId, ct);

        if (await uow.FlightAssignments.ExistsAsync(request.FlightId, request.StaffId, request.Id, ct))
            throw new Exception($"Staff with id {request.StaffId} is already assigned to flight with id {request.FlightId}.");

        flightAssignment.Update(request.FlightId, request.StaffId, request.FlightRoleId);

        await uow.FlightAssignments.UpdateAsync(flightAssignment, ct);
        await uow.SaveChangesAsync(ct);
    }

    private async Task ValidateReferences(int flightId, int staffId, int flightRoleId, CancellationToken ct)
    {
        if (await uow.Flights.GetByIdAsync(flightId, ct) is null)
            throw new Exception($"Flight with id {flightId} not found.");

        if (await uow.Staff.GetByIdAsync(staffId, ct) is null)
            throw new Exception($"Staff with id {staffId} not found.");

        if (await uow.FlightRoles.GetByIdAsync(flightRoleId, ct) is null)
            throw new Exception($"FlightRole with id {flightRoleId} not found.");
    }
}
