using MediatR;

namespace Application.UseCase.Clients;

public sealed record CreateClient(int PersonId) : IRequest<int>;
