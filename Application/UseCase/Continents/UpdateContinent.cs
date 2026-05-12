using MediatR;

namespace Application.UseCase.Continents;

public sealed record UpdateContinent(int Id, string Name) : IRequest;