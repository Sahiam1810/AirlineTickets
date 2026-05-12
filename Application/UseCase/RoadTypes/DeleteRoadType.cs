using MediatR;

namespace Application.UseCase.RoadTypes;

public sealed record DeleteRoadType(int Id) : IRequest;
