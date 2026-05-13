using MediatR;

namespace Application.UseCase.TicketStatuses;

public sealed record UpdateTicketStatus(int Id, string Name) : IRequest;
