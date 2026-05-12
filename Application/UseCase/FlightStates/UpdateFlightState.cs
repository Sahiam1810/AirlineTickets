using MediatR;

namespace Application.UseCase.FlightStates;

public sealed record UpdateFlightState(int Id, string Name) : IRequest;
