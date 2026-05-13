using Application.Abstractions;
using MediatR;

namespace Application.UseCase.ReservationFlights;

public sealed class UpdateReservationFlightHandler(IUnitOfWork uow) : IRequestHandler<UpdateReservationFlight>
{
    public async Task Handle(UpdateReservationFlight request, CancellationToken ct)
    {
        var reservationFlight = await uow.ReservationFlights.GetByIdAsync(request.Id, ct);

        if (reservationFlight is null)
        {
            throw new KeyNotFoundException($"ReservationFlight with id {request.Id} was not found.");
        }

        if (await uow.Reservations.GetByIdAsync(request.ReservationId, ct) is null)
        {
            throw new KeyNotFoundException($"Reservation with id {request.ReservationId} was not found.");
        }

        if (await uow.Flights.GetByIdAsync(request.FlightId, ct) is null)
        {
            throw new KeyNotFoundException($"Flight with id {request.FlightId} was not found.");
        }

        if (await uow.ReservationFlights.ExistsAsync(request.ReservationId, request.FlightId, request.Id, ct))
        {
            throw new InvalidOperationException("The flight is already associated with this reservation.");
        }

        reservationFlight.Update(
            request.ReservationId,
            request.FlightId,
            request.PartialValue);

        await uow.ReservationFlights.UpdateAsync(reservationFlight, ct);
        await uow.SaveChangesAsync(ct);
    }
}
