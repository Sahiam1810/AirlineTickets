using Application.Abstractions;
using Domain.ValueObjects.Staff;
using MediatR;

namespace Application.UseCase.StaffAvailabilities;

public sealed class UpdateStaffAvailabilityHandler(IUnitOfWork uow) : IRequestHandler<UpdateStaffAvailability>
{
    public async Task Handle(UpdateStaffAvailability request, CancellationToken ct)
    {
        _ = DateRange.Create(request.StartDate, request.EndDate);

        var staffAvailability = await uow.StaffAvailabilities.GetByIdAsync(request.Id, ct);
        if (staffAvailability is null)
            throw new Exception($"StaffAvailability with id {request.Id} not found.");

        var staff = await uow.Staff.GetByIdAsync(staffAvailability.StaffId, ct);
        if (staff is null)
            throw new Exception($"Staff with id {staffAvailability.StaffId} not found.");

        var availabilityStatus = await uow.AvailabilityStatuses.GetByIdAsync(request.AvailabilityStatusId, ct);
        if (availabilityStatus is null)
            throw new Exception($"AvailabilityStatus with id {request.AvailabilityStatusId} not found.");

        if (await uow.StaffAvailabilities.ExistsOverlappingAsync(
                staffAvailability.StaffId,
                request.StartDate,
                request.EndDate,
                request.Id,
                ct))
            throw new Exception("Staff availability range overlaps an existing range.");

        var notes = StaffAvailabilityNotes.Create(request.Notes);

        staffAvailability.Update(
            request.AvailabilityStatusId,
            request.StartDate,
            request.EndDate,
            notes);

        await uow.StaffAvailabilities.UpdateAsync(staffAvailability, ct);
        await uow.SaveChangesAsync(ct);
    }
}
