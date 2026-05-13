using MediatR;

namespace Application.UseCase.ReservationFlights;

public sealed record DeleteReservationFlight(int Id) : IRequest;
