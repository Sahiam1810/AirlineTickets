using FluentValidation;

namespace Application.UseCase.CabinConfigurations;

public sealed class CreateCabinConfigurationValidator : AbstractValidator<CreateCabinConfiguration>
{
    public CreateCabinConfigurationValidator()
    {
        RuleFor(x => x.AircraftId)
            .NotEmpty().WithMessage("Aircraft id is required.");

        RuleFor(x => x.CabinTypeId)
            .NotEmpty().WithMessage("Cabin type id is required.");

        RuleFor(x => x.RowStart)
            .GreaterThan(0).WithMessage("Row start must be greater than 0.");

        RuleFor(x => x.RowEnd)
            .GreaterThan(x => x.RowStart).WithMessage("Row end must be greater than row start.");

        RuleFor(x => x.SeatsPerRow)
            .GreaterThan(0).WithMessage("Seats per row must be greater than 0.");

        RuleFor(x => x.SeatLetters)
            .NotEmpty().WithMessage("Seat letters are required.")
            .MaximumLength(10)
            .Must((x, seatLetters) => !string.IsNullOrWhiteSpace(seatLetters) && seatLetters.Trim().Length == x.SeatsPerRow)
            .WithMessage("Seat letters length must match seats per row.");
    }
}
