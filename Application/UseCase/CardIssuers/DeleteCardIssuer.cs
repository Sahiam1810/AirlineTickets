using MediatR;

namespace Application.UseCase.CardIssuers;

public sealed record DeleteCardIssuer(int Id) : IRequest;
