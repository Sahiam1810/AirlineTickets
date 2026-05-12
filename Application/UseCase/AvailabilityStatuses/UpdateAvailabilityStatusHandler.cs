using Application.Abstractions;
using Domain.ValueObjects.Staff;
using MediatR;

namespace Application.UseCase.AvailabilityStatuses;

public sealed class UpdateAvailabilityStatusHandler(IUnitOfWork uow) : IRequestHandler<UpdateAvailabilityStatus>
{
    public async Task Handle(UpdateAvailabilityStatus request, CancellationToken ct)
    {
        var availabilityStatus = await uow.AvailabilityStatuses.GetByIdAsync(request.Id, ct);

        if (availabilityStatus is null)
            throw new Exception($"AvailabilityStatus with id {request.Id} not found.");

        var name = AvailabilityStatusName.Create(request.Name);
        var sameName = string.Equals(availabilityStatus.Name.Value, name.Value, StringComparison.OrdinalIgnoreCase);

        if (!sameName && await uow.AvailabilityStatuses.ExistsByNameAsync(name, ct))
            throw new Exception($"AvailabilityStatus with name {name.Value} already exists.");

        availabilityStatus.Update(name);

        await uow.AvailabilityStatuses.UpdateAsync(availabilityStatus, ct);
        await uow.SaveChangesAsync(ct);
    }
}
