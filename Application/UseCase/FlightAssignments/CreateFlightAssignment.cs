using MediatR;

namespace Application.UseCase.FlightAssignments;

public sealed record CreateFlightAssignment(int FlightId, int StaffId, int FlightRoleId) : IRequest<int>;
