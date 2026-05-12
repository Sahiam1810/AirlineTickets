using MediatR;

namespace Application.UseCase.Fares;

public sealed record CreateFare(
    int RouteId,
    int CabinTypeId,
    int PassengerTypeId,
    int SeasonId,
    decimal BasePrice,
    DateOnly? ValidFrom,
    DateOnly? ValidTo) : IRequest<int>;
