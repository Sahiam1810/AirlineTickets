using MediatR;

namespace Application.UseCase.FlightStatusTransitions;

public sealed record UpdateFlightStatusTransition(int Id, int FromStateId, int ToStateId) : IRequest;
