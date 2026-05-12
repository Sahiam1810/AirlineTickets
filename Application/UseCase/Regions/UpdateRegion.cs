using MediatR;

namespace Application.UseCase.Regions;

public sealed record UpdateRegion(int Id, string Name, string Type, int CountryId) : IRequest;
