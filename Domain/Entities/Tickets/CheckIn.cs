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
    public string BoardingPassNumber { get; private set; } = string.Empty;
    public bool HasCheckedBaggage { get; private set; }
    public decimal CheckedBaggageWeightKg { get; private set; }

    private CheckIn() { }

    public CheckIn(
        int ticketId,
        int staffId,
        int flightSeatId,
        DateTime checkInDate,
        int checkInStatusId,
        string boardingPassNumber,
        bool hasCheckedBaggage,
        decimal checkedBaggageWeightKg)
    {
        Validate(
            ticketId,
            staffId,
            flightSeatId,
            checkInDate,
            checkInStatusId,
            boardingPassNumber,
            hasCheckedBaggage,
            checkedBaggageWeightKg);

        TicketId = ticketId;
        StaffId = staffId;
        FlightSeatId = flightSeatId;
        CheckInDate = checkInDate;
        CheckInStatusId = checkInStatusId;
        BoardingPassNumber = boardingPassNumber.Trim();
        HasCheckedBaggage = hasCheckedBaggage;
        CheckedBaggageWeightKg = hasCheckedBaggage ? checkedBaggageWeightKg : 0;
    }

    public void Update(
        int ticketId,
        int staffId,
        int flightSeatId,
        DateTime checkInDate,
        int checkInStatusId,
        string boardingPassNumber,
        bool hasCheckedBaggage,
        decimal checkedBaggageWeightKg)
    {
        Validate(
            ticketId,
            staffId,
            flightSeatId,
            checkInDate,
            checkInStatusId,
            boardingPassNumber,
            hasCheckedBaggage,
            checkedBaggageWeightKg);

        TicketId = ticketId;
        StaffId = staffId;
        FlightSeatId = flightSeatId;
        CheckInDate = checkInDate;
        CheckInStatusId = checkInStatusId;
        BoardingPassNumber = boardingPassNumber.Trim();
        HasCheckedBaggage = hasCheckedBaggage;
        CheckedBaggageWeightKg = hasCheckedBaggage ? checkedBaggageWeightKg : 0;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void Validate(
        int ticketId,
        int staffId,
        int flightSeatId,
        DateTime checkInDate,
        int checkInStatusId,
        string boardingPassNumber,
        bool hasCheckedBaggage,
        decimal checkedBaggageWeightKg)
    {
        if (ticketId <= 0)
        {
            throw new ArgumentException("Ticket id must be greater than 0.", nameof(ticketId));
        }

        if (staffId <= 0)
        {
            throw new ArgumentException("Staff id must be greater than 0.", nameof(staffId));
        }

        if (flightSeatId <= 0)
        {
            throw new ArgumentException("Flight seat id must be greater than 0.", nameof(flightSeatId));
        }

        if (checkInDate == default)
        {
            throw new ArgumentException("Check-in date is required.", nameof(checkInDate));
        }

        if (checkInStatusId <= 0)
        {
            throw new ArgumentException("Check-in status id must be greater than 0.", nameof(checkInStatusId));
        }

        if (string.IsNullOrWhiteSpace(boardingPassNumber))
        {
            throw new ArgumentException("Boarding pass number is required.", nameof(boardingPassNumber));
        }

        if (boardingPassNumber.Trim().Length > 20)
        {
            throw new ArgumentException("Boarding pass number cannot exceed 20 characters.", nameof(boardingPassNumber));
        }

        if (checkedBaggageWeightKg < 0)
        {
            throw new ArgumentException("Checked baggage weight cannot be negative.", nameof(checkedBaggageWeightKg));
        }

        if (!hasCheckedBaggage && checkedBaggageWeightKg != 0)
        {
            throw new ArgumentException("Checked baggage weight must be 0 when there is no checked baggage.", nameof(checkedBaggageWeightKg));
        }
    }

    public Ticket Ticket { get; set; } = null!;
    public StaffMember Staff { get; set; } = null!;
    public FlightSeat FlightSeat { get; set; } = null!;
    public CheckInStatus CheckInStatus { get; set; } = null!;
}
