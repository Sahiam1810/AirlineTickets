using MediatR;

namespace Application.UseCase.FlightAssignments;

public sealed record UpdateFlightAssignment(int Id, int FlightId, int StaffId, int FlightRoleId) : IRequest;
