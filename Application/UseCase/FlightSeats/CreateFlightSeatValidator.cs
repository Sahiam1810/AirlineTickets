using FluentValidation;

namespace Application.UseCase.FlightSeats;

public sealed class CreateFlightSeatValidator : AbstractValidator<CreateFlightSeat>
{
    public CreateFlightSeatValidator()
    {
        RuleFor(x => x.FlightId).GreaterThan(0);
        RuleFor(x => x.SeatCode).NotEmpty().MaximumLength(5);
        RuleFor(x => x.SeatCode)
            .Must(BeValidSeatCode)
            .WithMessage("El codigo del asiento debe tener formato numero + letra, por ejemplo 12A.");
        RuleFor(x => x.CabinTypeId).GreaterThan(0);
        RuleFor(x => x.SeatLocationTypeId).GreaterThan(0);
    }

    private static bool BeValidSeatCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        var normalized = value.Trim();

        if (normalized.Length < 2 || normalized.Length > 5)
            return false;

        return normalized[..^1].All(char.IsDigit) && char.IsLetter(normalized[^1]);
    }
}
