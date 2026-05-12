using MediatR;

namespace Application.UseCase.Fares;

public sealed record DeleteFare(int Id) : IRequest;
