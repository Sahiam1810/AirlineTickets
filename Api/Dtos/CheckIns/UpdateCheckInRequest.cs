namespace Api.Dtos.CheckIns;

public sealed class UpdateCheckInRequest
{
    public int TicketId { get; init; }
    public int StaffId { get; init; }
    public int FlightSeatId { get; init; }
    public DateTime CheckInDate { get; init; }
    public int CheckInStatusId { get; init; }
    public string BoardingPassNumber { get; init; } = string.Empty;
    public bool HasCheckedBaggage { get; init; }
    public decimal CheckedBaggageWeightKg { get; init; }
}
