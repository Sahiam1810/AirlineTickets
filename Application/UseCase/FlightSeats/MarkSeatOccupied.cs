using MediatR;

namespace Application.UseCase.FlightSeats;

public sealed record MarkSeatOccupied(int Id) : IRequest;
