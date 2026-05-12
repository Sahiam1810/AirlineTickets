using MediatR;

namespace Application.UseCase.CabinTypes;

public sealed record UpdateCabinType(int Id, string Name) : IRequest;
