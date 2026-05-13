using MediatR;

namespace Application.UseCase.CardTypes;

public sealed record CreateCardType(string Name) : IRequest<int>;
