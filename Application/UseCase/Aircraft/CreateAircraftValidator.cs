using FluentValidation;

namespace Application.UseCase.Aircraft;

public sealed class CreateAircraftValidator : AbstractValidator<CreateAircraft>
{
    public CreateAircraftValidator()
    {
        RuleFor(x => x.AircraftModelId)
            .NotEmpty().WithMessage("Aircraft model id is required.");

        RuleFor(x => x.AirlineId)
            .NotEmpty().WithMessage("Airline id is required.");

        RuleFor(x => x.Registration)
            .NotEmpty().WithMessage("Registration is required.")
            .MaximumLength(20);

        RuleFor(x => x.ManufactureDate)
            .Must(x => !x.HasValue || x.Value <= DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Manufacture date cannot be in the future.");
    }
}
