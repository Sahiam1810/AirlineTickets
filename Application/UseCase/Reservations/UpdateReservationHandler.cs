using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Reservations;

public sealed class UpdateReservationHandler : IRequestHandler<UpdateReservation>
{
    private readonly IUnitOfWork _uow;

    public UpdateReservationHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task Handle(UpdateReservation req, CancellationToken ct)
    {
        var reservation = await _uow.Reservations.GetByIdAsync(req.Id, ct);
        if (reservation is null)
        {
            throw new KeyNotFoundException($"Reservation with id {req.Id} was not found.");
        }

        reservation.Update(req.ReservationStatusId, req.TotalValue, req.ExpiresAt);

        await _uow.Reservations.UpdateAsync(reservation, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
