using Application.Abstractions;
using Domain.Entities.Reservations;
using MediatR;

namespace Application.UseCase.ReservationFlights;

public sealed class CreateReservationFlightHandler(IUnitOfWork uow) : IRequestHandler<CreateReservationFlight, int>
{
    public async Task<int> Handle(CreateReservationFlight request, CancellationToken ct)
    {
        if (await uow.Reservations.GetByIdAsync(request.ReservationId, ct) is null)
        {
            throw new KeyNotFoundException($"Reservation with id {request.ReservationId} was not found.");
        }

        if (await uow.Flights.GetByIdAsync(request.FlightId, ct) is null)
        {
            throw new KeyNotFoundException($"Flight with id {request.FlightId} was not found.");
        }

        if (await uow.ReservationFlights.ExistsAsync(request.ReservationId, request.FlightId, ct: ct))
        {
            throw new InvalidOperationException("The flight is already associated with this reservation.");
        }

        var reservationFlight = new ReservationFlight(
            request.ReservationId,
            request.FlightId,
            request.PartialValue);

        await uow.ReservationFlights.AddAsync(reservationFlight, ct);
        await uow.SaveChangesAsync(ct);

        return reservationFlight.Id;
    }
}
