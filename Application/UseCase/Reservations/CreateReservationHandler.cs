using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities.Reservations;
using MediatR;

namespace Application.UseCase.Reservations;

public sealed class CreateReservationHandler : IRequestHandler<CreateReservation, int>
{
    private readonly IUnitOfWork _uow;

    public CreateReservationHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<int> Handle(CreateReservation req, CancellationToken ct)
    {
        if (await _uow.Reservations.ExistsCodeAsync(req.ReservationCode, ct))
        {
            throw new InvalidOperationException($"Reservation code '{req.ReservationCode}' already exists.");
        }

        var reservation = new Reservation(
            req.ReservationCode,
            req.ClientId,
            req.ReservationStatusId,
            req.TotalValue,
            req.ExpiresAt);

        await _uow.Reservations.AddAsync(reservation, ct);
        await _uow.SaveChangesAsync(ct);

        return reservation.Id;
    }
}
