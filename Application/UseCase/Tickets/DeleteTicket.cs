using MediatR;

namespace Application.UseCase.Tickets;

public sealed record DeleteTicket(int Id) : IRequest;
