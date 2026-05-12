using Application.Abstractions;
using MediatR;

namespace Application.UseCase.AvailabilityStatuses;

public sealed class DeleteAvailabilityStatusHandler(IUnitOfWork uow) : IRequestHandler<DeleteAvailabilityStatus>
{
    public async Task Handle(DeleteAvailabilityStatus request, CancellationToken ct)
    {
        var availabilityStatus = await uow.AvailabilityStatuses.GetByIdAsync(request.Id, ct);

        if (availabilityStatus is null)
            throw new Exception($"AvailabilityStatus with id {request.Id} not found.");

        await uow.AvailabilityStatuses.RemoveAsync(availabilityStatus, ct);
        await uow.SaveChangesAsync(ct);
    }
}
