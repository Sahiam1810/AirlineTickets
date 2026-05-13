using MediatR;

namespace Application.UseCase.CardTypes;

public sealed record UpdateCardType(int Id, string Name) : IRequest;
