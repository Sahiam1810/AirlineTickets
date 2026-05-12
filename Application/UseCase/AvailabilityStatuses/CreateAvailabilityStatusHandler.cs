using Application.Abstractions;
using Domain.Entities.Staff;
using Domain.ValueObjects.Staff;
using MediatR;

namespace Application.UseCase.AvailabilityStatuses;

public sealed class CreateAvailabilityStatusHandler : IRequestHandler<CreateAvailabilityStatus, int>
{
    private readonly IUnitOfWork uow;

    public CreateAvailabilityStatusHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateAvailabilityStatus req, CancellationToken ct)
    {
        var name = AvailabilityStatusName.Create(req.Name);

        if (await uow.AvailabilityStatuses.ExistsByNameAsync(name, ct))
            throw new Exception($"AvailabilityStatus with name {name.Value} already exists.");

        var availabilityStatus = new AvailabilityStatus(name);

        await uow.AvailabilityStatuses.AddAsync(availabilityStatus, ct);
        await uow.SaveChangesAsync(ct);
        return availabilityStatus.Id;
    }
}
