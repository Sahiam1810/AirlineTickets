using MediatR;

namespace Application.UseCase.Airlines;

public sealed record CreateAirline(string Name, string IataCode, int CountryId, bool IsActive) : IRequest<int>;
