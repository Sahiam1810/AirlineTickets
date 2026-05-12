using MediatR;

namespace Application.UseCase.Fares;

public sealed record UpdateFare(
    int Id,
    int RouteId,
    int CabinTypeId,
    int PassengerTypeId,
    int SeasonId,
    decimal BasePrice,
    DateOnly? ValidFrom,
    DateOnly? ValidTo) : IRequest;
