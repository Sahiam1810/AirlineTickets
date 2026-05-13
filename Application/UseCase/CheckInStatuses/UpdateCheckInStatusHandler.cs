using Application.Abstractions;
using MediatR;

namespace Application.UseCase.CheckInStatuses;

public sealed class UpdateCheckInStatusHandler(IUnitOfWork uow) : IRequestHandler<UpdateCheckInStatus>
{
    public async Task Handle(UpdateCheckInStatus request, CancellationToken ct)
    {
        var checkInStatus = await uow.CheckInStatuses.GetByIdAsync(request.Id, ct);

        if (checkInStatus is null)
        {
            throw new KeyNotFoundException($"CheckInStatus with id {request.Id} was not found.");
        }

        var name = request.Name.Trim();
        var sameName = string.Equals(checkInStatus.Name, name, StringComparison.OrdinalIgnoreCase);

        if (!sameName && await uow.CheckInStatuses.ExistsAsync(name, request.Id, ct))
        {
            throw new InvalidOperationException($"CheckInStatus with name {name} already exists.");
        }

        checkInStatus.Update(name);

        await uow.CheckInStatuses.UpdateAsync(checkInStatus, ct);
        await uow.SaveChangesAsync(ct);
    }
}
