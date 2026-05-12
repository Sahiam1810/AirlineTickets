using Application.Abstractions;
using Domain.Entities.Routes;
using Domain.ValueObjects.Fares;
using MediatR;

namespace Application.UseCase.Fares;

public sealed class CreateFareHandler : IRequestHandler<CreateFare, int>
{
    private readonly IUnitOfWork uow;

    public CreateFareHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateFare req, CancellationToken ct)
    {
        _ = BasePrice.Create(req.BasePrice);
        DateRange.Validate(req.ValidFrom, req.ValidTo);

        await ValidateReferences(req.RouteId, req.CabinTypeId, req.PassengerTypeId, req.SeasonId, ct);

        if (await uow.Fares.ExistsAsync(
            req.RouteId,
            req.CabinTypeId,
            req.PassengerTypeId,
            req.SeasonId,
            req.ValidFrom,
            req.ValidTo,
            null,
            ct))
        {
            throw new Exception("Fare already exists for this route, cabin type, passenger type, season and date range.");
        }

        var fare = new Fare(
            req.RouteId,
            req.CabinTypeId,
            req.PassengerTypeId,
            req.SeasonId,
            req.BasePrice,
            req.ValidFrom,
            req.ValidTo);

        await uow.Fares.AddAsync(fare, ct);
        await uow.SaveChangesAsync(ct);
        return fare.Id;
    }

    private async Task ValidateReferences(
        int routeId,
        int cabinTypeId,
        int passengerTypeId,
        int seasonId,
        CancellationToken ct)
    {
        if (await uow.Routes.GetByIdAsync(routeId, ct) is null)
            throw new Exception($"Route with id {routeId} not found.");

        if (await uow.CabinTypes.GetByIdAsync(cabinTypeId, ct) is null)
            throw new Exception($"CabinType with id {cabinTypeId} not found.");

        if (await uow.PassengerTypes.GetByIdAsync(passengerTypeId, ct) is null)
            throw new Exception($"PassengerType with id {passengerTypeId} not found.");

        if (await uow.Seasons.GetByIdAsync(seasonId, ct) is null)
            throw new Exception($"Season with id {seasonId} not found.");
    }
}
