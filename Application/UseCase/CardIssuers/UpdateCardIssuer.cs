using MediatR;

namespace Application.UseCase.CardIssuers;

public sealed record UpdateCardIssuer(int Id, string Name) : IRequest;
