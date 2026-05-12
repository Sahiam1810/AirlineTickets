using Application.Abstractions;
using Domain.Entities.Staff;
using Domain.ValueObjects.Staff;
using MediatR;

namespace Application.UseCase.StaffAvailabilities;

public sealed class CreateStaffAvailabilityHandler : IRequestHandler<CreateStaffAvailability, int>
{
    private readonly IUnitOfWork uow;

    public CreateStaffAvailabilityHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateStaffAvailability req, CancellationToken ct)
    {
        _ = DateRange.Create(req.StartDate, req.EndDate);

        var staff = await uow.Staff.GetByIdAsync(req.StaffId, ct);
        if (staff is null)
            throw new Exception($"Staff with id {req.StaffId} not found.");

        var availabilityStatus = await uow.AvailabilityStatuses.GetByIdAsync(req.AvailabilityStatusId, ct);
        if (availabilityStatus is null)
            throw new Exception($"AvailabilityStatus with id {req.AvailabilityStatusId} not found.");

        if (await uow.StaffAvailabilities.ExistsOverlappingAsync(req.StaffId, req.StartDate, req.EndDate, null, ct))
            throw new Exception("Staff availability range overlaps an existing range.");

        var notes = StaffAvailabilityNotes.Create(req.Notes);
        var staffAvailability = new StaffAvailability(
            req.StaffId,
            req.AvailabilityStatusId,
            req.StartDate,
            req.EndDate,
            notes);

        await uow.StaffAvailabilities.AddAsync(staffAvailability, ct);
        await uow.SaveChangesAsync(ct);
        return staffAvailability.Id;
    }
}
