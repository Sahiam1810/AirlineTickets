using MediatR;

namespace Application.UseCase.Airlines;

public sealed record UpdateAirline(int Id, string Name, string IataCode, int CountryId, bool IsActive) : IRequest;
