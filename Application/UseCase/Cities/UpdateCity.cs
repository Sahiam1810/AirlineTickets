using MediatR;

namespace Application.UseCase.Cities;

public sealed record UpdateCity(int Id, string Name, int RegionId) : IRequest;
