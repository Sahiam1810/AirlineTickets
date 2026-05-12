using MediatR;

namespace Application.UseCase.Airports;

public sealed record CreateAirport(string Name, string IataCode, string? IcaoCode, int CityId) : IRequest<int>;
