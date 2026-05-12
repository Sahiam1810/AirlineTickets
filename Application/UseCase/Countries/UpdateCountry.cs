using MediatR;

namespace Application.UseCase.Countries;

public sealed record UpdateCountry(int Id, string Name, int ContinentId) : IRequest;
