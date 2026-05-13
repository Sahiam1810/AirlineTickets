using MediatR;

namespace Application.UseCase.FlightStatusTransitions;

public sealed record CreateFlightStatusTransition(int FromStateId, int ToStateId) : IRequest<int>;
