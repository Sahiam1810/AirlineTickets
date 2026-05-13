using MediatR;

namespace Application.UseCase.FlightSeats;

public sealed record MarkSeatAvailable(int Id) : IRequest;
