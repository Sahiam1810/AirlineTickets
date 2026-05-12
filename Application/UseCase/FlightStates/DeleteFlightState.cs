using MediatR;

namespace Application.UseCase.FlightStates;

public sealed record DeleteFlightState(int Id) : IRequest;
