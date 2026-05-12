using MediatR;

namespace Application.UseCase.Seasons;

public sealed record CreateSeason(string Name, string? Description, decimal PriceFactor) : IRequest<int>;
