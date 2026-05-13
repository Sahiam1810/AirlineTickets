using MediatR;

namespace Application.UseCase.CardTypes;

public sealed record DeleteCardType(int Id) : IRequest;
