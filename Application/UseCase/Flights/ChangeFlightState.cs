using MediatR;

namespace Application.UseCase.Flights;

public sealed record ChangeFlightState(int Id, int FlightStateId) : IRequest;
