using MediatR;

namespace Application.UseCase.CabinTypes;

public sealed record DeleteCabinType(int Id) : IRequest;
