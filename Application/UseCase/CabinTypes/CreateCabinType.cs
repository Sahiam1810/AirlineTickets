using MediatR;

namespace Application.UseCase.CabinTypes;

public sealed record CreateCabinType(string Name) : IRequest<int>;
