using FluentValidation;

namespace Application.UseCase.AirportAirlines;

public sealed class CreateAirportAirlineValidator : AbstractValidator<CreateAirportAirline>
{
    public CreateAirportAirlineValidator()
    {
        RuleFor(x => x.AirportId)
            .NotEmpty().WithMessage("Airport id is required.");

        RuleFor(x => x.AirlineId)
            .NotEmpty().WithMessage("Airline id is required.");

        RuleFor(x => x.Terminal)
            .MaximumLength(20);

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.");
    }
}
