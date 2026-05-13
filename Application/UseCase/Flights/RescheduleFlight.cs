using MediatR;

namespace Application.UseCase.Flights;

public sealed record RescheduleFlight(int Id, DateTime DepartureDate, DateTime EstimatedArrivalDate) : IRequest;
