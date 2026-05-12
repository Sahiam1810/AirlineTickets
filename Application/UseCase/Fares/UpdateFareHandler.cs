using Application.Abstractions;
using Domain.ValueObjects.Fares;
using MediatR;

namespace Application.UseCase.Fares;

public sealed class UpdateFareHandler(IUnitOfWork uow) : IRequestHandler<UpdateFare>
{
    public async Task Handle(UpdateFare request, CancellationToken ct)
    {
        var fare = await uow.Fares.GetByIdAsync(request.Id, ct);

        if (fare is null)
            throw new Exception($"Fare with id {request.Id} not found.");

        _ = BasePrice.Create(request.BasePrice);
        DateRange.Validate(request.ValidFrom, request.ValidTo);

        await ValidateReferences(
            request.RouteId,
            request.CabinTypeId,
            request.PassengerTypeId,
            request.SeasonId,
            ct);

        if (await uow.Fares.ExistsAsync(
            request.RouteId,
            request.CabinTypeId,
            request.PassengerTypeId,
            request.SeasonId,
            request.ValidFrom,
            request.ValidTo,
            request.Id,
            ct))
        {
            throw new Exception("Fare already exists for this route, cabin type, passenger type, season and date range.");
        }

        fare.Update(
            request.RouteId,
            request.CabinTypeId,
            request.PassengerTypeId,
            request.SeasonId,
            request.BasePrice,
            request.ValidFrom,
            request.ValidTo);

        await uow.Fares.UpdateAsync(fare, ct);
        await uow.SaveChangesAsync(ct);
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
