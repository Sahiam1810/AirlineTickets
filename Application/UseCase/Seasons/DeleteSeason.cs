using MediatR;

namespace Application.UseCase.Seasons;

public sealed record DeleteSeason(int Id) : IRequest;
