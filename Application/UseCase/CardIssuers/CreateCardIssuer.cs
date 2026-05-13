using MediatR;

namespace Application.UseCase.CardIssuers;

public sealed record CreateCardIssuer(string Name) : IRequest<int>;
