using MediatR;

namespace Application.UseCase.Cities;

public sealed record CreateCity(string Name, int RegionId) : IRequest<int>;
