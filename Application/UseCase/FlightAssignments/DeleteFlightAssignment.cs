using MediatR;

namespace Application.UseCase.FlightAssignments;

public sealed record DeleteFlightAssignment(int Id) : IRequest;
