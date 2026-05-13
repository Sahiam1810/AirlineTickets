using MediatR;

namespace Application.UseCase.FlightRoles;

public sealed record DeleteFlightRole(int Id) : IRequest;
