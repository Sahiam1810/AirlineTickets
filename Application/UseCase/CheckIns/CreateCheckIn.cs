using MediatR;

namespace Application.UseCase.CheckIns;

public sealed record CreateCheckIn(
    int TicketId,
    int StaffId,
    int FlightSeatId,
    DateTime CheckInDate,
    int CheckInStatusId,
    string BoardingPassNumber,
    bool HasCheckedBaggage,
    decimal CheckedBaggageWeightKg) : IRequest<int>;
