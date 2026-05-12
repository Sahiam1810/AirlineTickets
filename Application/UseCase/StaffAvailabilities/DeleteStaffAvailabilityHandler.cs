using Application.Abstractions;
using MediatR;

namespace Application.UseCase.StaffAvailabilities;

public sealed class DeleteStaffAvailabilityHandler(IUnitOfWork uow) : IRequestHandler<DeleteStaffAvailability>
{
    public async Task Handle(DeleteStaffAvailability request, CancellationToken ct)
    {
        var staffAvailability = await uow.StaffAvailabilities.GetByIdAsync(request.Id, ct);

        if (staffAvailability is null)
            throw new Exception($"StaffAvailability with id {request.Id} not found.");

        await uow.StaffAvailabilities.RemoveAsync(staffAvailability, ct);
        await uow.SaveChangesAsync(ct);
    }
}
