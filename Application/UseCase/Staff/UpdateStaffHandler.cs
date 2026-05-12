using Application.Abstractions;
using Domain.ValueObjects.Staff;
using MediatR;

namespace Application.UseCase.Staff;

public sealed class UpdateStaffHandler(IUnitOfWork uow) : IRequestHandler<UpdateStaff>
{
    public async Task Handle(UpdateStaff request, CancellationToken ct)
    {
        var staff = await uow.Staff.GetByIdAsync(request.Id, ct);

        if (staff is null)
            throw new Exception($"Staff with id {request.Id} not found.");

        var staffRole = await uow.StaffRoles.GetByIdAsync(request.StaffRoleId, ct);
        if (staffRole is null)
            throw new Exception($"StaffRole with id {request.StaffRoleId} not found.");

        await ValidateAssignmentAsync(request.AirlineId, request.AirportId, ct);

        var hireDate = HireDate.Create(request.HireDate);

        staff.Update(request.StaffRoleId, request.AirlineId, request.AirportId, hireDate, request.IsActive);

        await uow.Staff.UpdateAsync(staff, ct);
        await uow.SaveChangesAsync(ct);
    }

    private async Task ValidateAssignmentAsync(int? airlineId, int? airportId, CancellationToken ct)
    {
        if (!airlineId.HasValue && !airportId.HasValue)
            throw new Exception("Airline id or airport id is required.");

        if (airlineId.HasValue && airportId.HasValue)
            throw new Exception("Staff cannot belong to an airline and an airport at the same time.");

        if (airlineId.HasValue && await uow.Airlines.GetByIdAsync(airlineId.Value, ct) is null)
            throw new Exception($"Airline with id {airlineId.Value} not found.");

        if (airportId.HasValue && await uow.Airports.GetByIdAsync(airportId.Value, ct) is null)
            throw new Exception($"Airport with id {airportId.Value} not found.");
    }
}
