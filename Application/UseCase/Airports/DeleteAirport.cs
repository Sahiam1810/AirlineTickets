using MediatR;

namespace Application.UseCase.Airports;

public sealed record DeleteAirport(int Id) : IRequest;
