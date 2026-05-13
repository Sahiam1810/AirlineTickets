using Application.Abstractions;
using Domain.Entities.Flights;
using Domain.ValueObjects.FlightSeats;
using MediatR;

namespace Application.UseCase.FlightSeats;

public sealed class CreateFlightSeatHandler : IRequestHandler<CreateFlightSeat, int>
{
    private readonly IUnitOfWork uow;

    public CreateFlightSeatHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateFlightSeat req, CancellationToken ct)
    {
        var seatCode = SeatCode.Create(req.SeatCode);

        await ValidateReferences(req.FlightId, req.CabinTypeId, req.SeatLocationTypeId, ct);

        if (await uow.FlightSeats.ExistsAsync(req.FlightId, seatCode, null, ct))
            throw new Exception($"FlightSeat with code {seatCode.Value} already exists for flight with id {req.FlightId}.");

        var flightSeat = new FlightSeat(
            req.FlightId,
            req.SeatCode,
            req.CabinTypeId,
            req.SeatLocationTypeId,
            req.IsOccupied);

        await uow.FlightSeats.AddAsync(flightSeat, ct);
        await uow.SaveChangesAsync(ct);
        return flightSeat.Id;
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
