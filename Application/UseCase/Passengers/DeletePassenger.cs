using MediatR;

namespace Application.UseCase.Passengers;

public sealed record DeletePassenger(int Id) : IRequest;
