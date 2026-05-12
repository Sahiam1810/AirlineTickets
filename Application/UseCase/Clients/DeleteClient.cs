using MediatR;

namespace Application.UseCase.Clients;

public sealed record DeleteClient(int Id) : IRequest;
