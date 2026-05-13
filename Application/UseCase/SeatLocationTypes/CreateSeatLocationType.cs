using MediatR;

namespace Application.UseCase.SeatLocationTypes;

public sealed record CreateSeatLocationType(string Name) : IRequest<int>;
