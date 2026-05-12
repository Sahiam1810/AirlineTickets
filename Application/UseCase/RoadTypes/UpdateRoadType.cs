using MediatR;

namespace Application.UseCase.RoadTypes;

public sealed record UpdateRoadType(int Id, string Name) : IRequest;
