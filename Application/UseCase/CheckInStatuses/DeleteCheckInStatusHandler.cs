using Application.Abstractions;
using MediatR;

namespace Application.UseCase.CheckInStatuses;

public sealed class DeleteCheckInStatusHandler(IUnitOfWork uow) : IRequestHandler<DeleteCheckInStatus>
{
    public async Task Handle(DeleteCheckInStatus request, CancellationToken ct)
    {
        var checkInStatus = await uow.CheckInStatuses.GetByIdAsync(request.Id, ct);

        if (checkInStatus is null)
        {
            throw new KeyNotFoundException($"CheckInStatus with id {request.Id} was not found.");
        }

        await uow.CheckInStatuses.RemoveAsync(checkInStatus, ct);
        await uow.SaveChangesAsync(ct);
    }
}
