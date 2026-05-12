using MediatR;

namespace Application.UseCase.Aircraft;

public sealed record DeleteAircraft(int Id) : IRequest;
