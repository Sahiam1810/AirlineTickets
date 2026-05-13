using System.Text.RegularExpressions;

namespace Domain.ValueObjects.Reservations;

public sealed class ReservationCode
{
    public string Value { get; }

    private ReservationCode(string value)
    {
        Value = value;
    }

    public static ReservationCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Reservation code cannot be null or empty.");

        if (value.Length > 30)
            throw new ArgumentException("Reservation code cannot exceed 30 characters.");

        if (!Regex.IsMatch(value, @"^[A-Z0-9]+$"))
            throw new ArgumentException("Reservation code must contain only uppercase letters and numbers.");

        return new ReservationCode(value);
    }

    public override string ToString() => Value;
}
