using MediatR;

namespace Application.UseCase.CheckIns;

public sealed record DeleteCheckIn(int Id) : IRequest;
