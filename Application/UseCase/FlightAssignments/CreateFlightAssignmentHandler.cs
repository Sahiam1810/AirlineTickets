using Application.Abstractions;
using Domain.Entities.Flights;
using MediatR;

namespace Application.UseCase.FlightAssignments;

public sealed class CreateFlightAssignmentHandler : IRequestHandler<CreateFlightAssignment, int>
{
    private readonly IUnitOfWork uow;

    public CreateFlightAssignmentHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateFlightAssignment req, CancellationToken ct)
    {
        await ValidateReferences(req.FlightId, req.StaffId, req.FlightRoleId, ct);

        if (await uow.FlightAssignments.ExistsAsync(req.FlightId, req.StaffId, null, ct))
            throw new Exception($"Staff with id {req.StaffId} is already assigned to flight with id {req.FlightId}.");

        var flightAssignment = new FlightAssignment(req.FlightId, req.StaffId, req.FlightRoleId);

        await uow.FlightAssignments.AddAsync(flightAssignment, ct);
        await uow.SaveChangesAsync(ct);
        return flightAssignment.Id;
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
