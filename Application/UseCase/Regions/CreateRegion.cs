using MediatR;

namespace Application.UseCase.Regions;

public sealed record CreateRegion(string Name, string Type, int CountryId) : IRequest<int>;
