using MediatR;

namespace Application.UseCase.Flights;

public sealed record DeleteFlight(int Id) : IRequest;
