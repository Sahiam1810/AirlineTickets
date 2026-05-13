namespace Api.Dtos.CheckIns;

public sealed class CheckInDto
{
    public int Id { get; init; }
    public int TicketId { get; init; }
    public string TicketCode { get; init; } = string.Empty;
    public int StaffId { get; init; }
    public string StaffName { get; init; } = string.Empty;
    public int FlightSeatId { get; init; }
    public string SeatCode { get; init; } = string.Empty;
    public int CheckInStatusId { get; init; }
    public string CheckInStatusName { get; init; } = string.Empty;
    public DateTime CheckInDate { get; init; }
    public string BoardingPassNumber { get; init; } = string.Empty;
    public bool HasCheckedBaggage { get; init; }
    public decimal CheckedBaggageWeightKg { get; init; }
    public DateTime CreatedAt { get; init; }
}
