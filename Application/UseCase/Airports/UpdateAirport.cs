using MediatR;

namespace Application.UseCase.Airports;

public sealed record UpdateAirport(int Id, string Name, string IataCode, string? IcaoCode, int CityId) : IRequest;
