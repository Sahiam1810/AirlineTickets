using MediatR;

namespace Application.UseCase.FlightSeats;

public sealed record DeleteFlightSeat(int Id) : IRequest;
