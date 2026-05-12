using Application.Abstractions;
using Domain.Entities.Staff;
using Domain.ValueObjects.Staff;
using MediatR;

namespace Application.UseCase.Staff;

public sealed class CreateStaffHandler : IRequestHandler<CreateStaff, int>
{
    private readonly IUnitOfWork uow;

    public CreateStaffHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateStaff req, CancellationToken ct)
    {
        var person = await uow.People.GetByIdAsync(req.PersonId, ct);
        if (person is null)
            throw new Exception($"Person with id {req.PersonId} not found.");

        var staffRole = await uow.StaffRoles.GetByIdAsync(req.StaffRoleId, ct);
        if (staffRole is null)
            throw new Exception($"StaffRole with id {req.StaffRoleId} not found.");

        if (await uow.Staff.ExistsByPersonIdAsync(req.PersonId, ct))
            throw new Exception($"Staff with person id {req.PersonId} already exists.");

        await ValidateAssignmentAsync(req.AirlineId, req.AirportId, ct);

        var hireDate = HireDate.Create(req.HireDate);
        var staff = new StaffMember(req.PersonId, req.StaffRoleId, req.AirlineId, req.AirportId, hireDate, req.IsActive);

        await uow.Staff.AddAsync(staff, ct);
        await uow.SaveChangesAsync(ct);
        return staff.Id;
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
