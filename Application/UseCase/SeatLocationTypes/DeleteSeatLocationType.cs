using MediatR;

namespace Application.UseCase.SeatLocationTypes;

public sealed record DeleteSeatLocationType(int Id) : IRequest;
