using MediatR;

namespace Application.UseCase.Seasons;

public sealed record UpdateSeason(int Id, string Name, string? Description, decimal PriceFactor) : IRequest;
