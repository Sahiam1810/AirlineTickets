using Application.Abstractions;
using Domain.ValueObjects.FlightSeats;
using MediatR;

namespace Application.UseCase.FlightSeats;

public sealed class UpdateFlightSeatHandler(IUnitOfWork uow) : IRequestHandler<UpdateFlightSeat>
{
    public async Task Handle(UpdateFlightSeat request, CancellationToken ct)
    {
        var flightSeat = await uow.FlightSeats.GetByIdAsync(request.Id, ct);

        if (flightSeat is null)
            throw new Exception($"FlightSeat with id {request.Id} not found.");

        var seatCode = SeatCode.Create(request.SeatCode);

        await ValidateReferences(request.FlightId, request.CabinTypeId, request.SeatLocationTypeId, ct);

        if (await uow.FlightSeats.ExistsAsync(request.FlightId, seatCode, request.Id, ct))
            throw new Exception($"FlightSeat with code {seatCode.Value} already exists for flight with id {request.FlightId}.");

        flightSeat.Update(
            request.FlightId,
            request.SeatCode,
            request.CabinTypeId,
            request.SeatLocationTypeId,
            request.IsOccupied);

        await uow.FlightSeats.UpdateAsync(flightSeat, ct);
        await uow.SaveChangesAsync(ct);
    }

    private async Task ValidateReferences(int flightId, int cabinTypeId, int seatLocationTypeId, CancellationToken ct)
    {
        if (await uow.Flights.GetByIdAsync(flightId, ct) is null)
            throw new Exception($"Flight with id {flightId} not found.");

        if (await uow.CabinTypes.GetByIdAsync(cabinTypeId, ct) is null)
            throw new Exception($"CabinType with id {cabinTypeId} not found.");

        if (await uow.SeatLocationTypes.GetByIdAsync(seatLocationTypeId, ct) is null)
            throw new Exception($"SeatLocationType with id {seatLocationTypeId} not found.");
    }
}
