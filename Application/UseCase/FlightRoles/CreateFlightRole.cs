using MediatR;

namespace Application.UseCase.FlightRoles;

public sealed record CreateFlightRole(string Name) : IRequest<int>;
