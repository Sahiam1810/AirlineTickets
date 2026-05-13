using MediatR;

namespace Application.UseCase.FlightStatusTransitions;

public sealed record DeleteFlightStatusTransition(int Id) : IRequest;
