using Application.Abstractions;
using Domain.Entities.Tickets;
using MediatR;

namespace Application.UseCase.CheckInStatuses;

public sealed class CreateCheckInStatusHandler(IUnitOfWork uow) : IRequestHandler<CreateCheckInStatus, int>
{
    public async Task<int> Handle(CreateCheckInStatus request, CancellationToken ct)
    {
        var name = request.Name.Trim();

        if (await uow.CheckInStatuses.ExistsAsync(name, null, ct))
        {
            throw new InvalidOperationException($"CheckInStatus with name {name} already exists.");
        }

        var checkInStatus = new CheckInStatus(name);

        await uow.CheckInStatuses.AddAsync(checkInStatus, ct);
        await uow.SaveChangesAsync(ct);

        return checkInStatus.Id;
    }
}
