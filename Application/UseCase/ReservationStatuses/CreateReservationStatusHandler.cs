using Application.Abstractions;
using Domain.Entities.Reservations;
using Domain.ValueObjects.ReservationStatuses;
using MediatR;

namespace Application.UseCase.ReservationStatuses;

public sealed class CreateReservationStatusHandler : IRequestHandler<CreateReservationStatus, int>
{
    private readonly IUnitOfWork uow;

    public CreateReservationStatusHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateReservationStatus req, CancellationToken ct)
    {
        var name = ReservationStatusName.Create(req.Name);

        if (await uow.ReservationStatuses.ExistsByNameAsync(name, null, ct))
            throw new Exception($"ReservationStatus with name {name.Value} already exists.");

        var reservationStatus = new ReservationStatus(req.Name);

        await uow.ReservationStatuses.AddAsync(reservationStatus, ct);
        await uow.SaveChangesAsync(ct);
        return reservationStatus.Id;
    }
}
