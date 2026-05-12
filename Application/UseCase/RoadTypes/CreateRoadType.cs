using MediatR;

namespace Application.UseCase.RoadTypes;

public sealed record CreateRoadType(string Name) : IRequest<int>;
