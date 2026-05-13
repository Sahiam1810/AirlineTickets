using MediatR;

namespace Application.UseCase.CheckIns;

public sealed record UpdateCheckIn(
    int Id,
    int TicketId,
    int StaffId,
    int FlightSeatId,
    DateTime CheckInDate,
    int CheckInStatusId,
    string BoardingPassNumber,
    bool HasCheckedBaggage,
    decimal CheckedBaggageWeightKg) : IRequest;
