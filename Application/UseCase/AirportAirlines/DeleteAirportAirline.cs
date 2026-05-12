using MediatR;

namespace Application.UseCase.AirportAirlines;

public sealed record DeleteAirportAirline(int Id) : IRequest;
