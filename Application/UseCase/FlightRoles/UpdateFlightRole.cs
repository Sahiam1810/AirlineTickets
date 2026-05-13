using MediatR;

namespace Application.UseCase.FlightRoles;

public sealed record UpdateFlightRole(int Id, string Name) : IRequest;
