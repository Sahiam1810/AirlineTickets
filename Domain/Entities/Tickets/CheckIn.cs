using System;
using Domain.Common;
using Domain.Entities.Flights;
using Domain.Entities.Staff;

namespace Domain.Entities.Tickets;

public sealed class CheckIn : BaseEntity<int>
{
    public int TicketId { get; private set; }
    public int StaffId { get; private set; }
    public int FlightSeatId { get; private set; }
    public DateTime CheckInDate { get; private set; }
    public int CheckInStatusId { get; private set; }
    public string BoardingCardNumber { get; private set; } = string.Empty;
    public bool HasCheckedBaggage { get; private set; }
    public decimal? BaggageWeightKg { get; private set; }

    // Navigation
    public Ticket Ticket { get; set; } = null!;
    public StaffMember Staff { get; set; } = null!;
    public FlightSeat FlightSeat { get; set; } = null!;
    public CheckInStatus CheckInStatus { get; set; } = null!;

    private CheckIn() { }

    public CheckIn(int ticketId, int staffId, int flightSeatId, DateTime checkInDate,
        int checkInStatusId, string boardingCardNumber, bool hasCheckedBaggage, decimal? baggageWeightKg)
    {
        TicketId = ticketId;
        StaffId = staffId;
        FlightSeatId = flightSeatId;
        CheckInDate = checkInDate;
        CheckInStatusId = checkInStatusId;
        BoardingCardNumber = boardingCardNumber;
        HasCheckedBaggage = hasCheckedBaggage;
        BaggageWeightKg = baggageWeightKg;
    }

    public void Update(int checkInStatusId, bool hasCheckedBaggage, decimal? baggageWeightKg)
    {
        CheckInStatusId = checkInStatusId;
        HasCheckedBaggage = hasCheckedBaggage;
        BaggageWeightKg = baggageWeightKg;
    }
}