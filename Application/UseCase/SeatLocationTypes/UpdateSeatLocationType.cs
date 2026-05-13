using MediatR;

namespace Application.UseCase.SeatLocationTypes;

public sealed record UpdateSeatLocationType(int Id, string Name) : IRequest;
