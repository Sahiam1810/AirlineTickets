using MediatR;

namespace Application.UseCase.FlightStates;

public sealed record CreateFlightState(string Name) : IRequest<int>;
