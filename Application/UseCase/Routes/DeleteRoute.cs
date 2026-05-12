using MediatR;

namespace Application.UseCase.Routes;

public sealed record DeleteRoute(int Id) : IRequest;
