using MediatR;

namespace Application.UseCase.Countries;

public sealed record CreateCountry(string Name, int ContinentId) : IRequest<int>;
