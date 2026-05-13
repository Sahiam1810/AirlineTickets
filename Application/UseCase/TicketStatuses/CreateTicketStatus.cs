using MediatR;

namespace Application.UseCase.TicketStatuses;

public sealed record CreateTicketStatus(string Name) : IRequest<int>;
