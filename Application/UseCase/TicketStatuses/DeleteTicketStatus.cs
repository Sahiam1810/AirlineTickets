using MediatR;

namespace Application.UseCase.TicketStatuses;

public sealed record DeleteTicketStatus(int Id) : IRequest;
