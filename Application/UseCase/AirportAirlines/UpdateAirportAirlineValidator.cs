using FluentValidation;

namespace Application.UseCase.AirportAirlines;

public sealed class UpdateAirportAirlineValidator : AbstractValidator<UpdateAirportAirline>
{
    public UpdateAirportAirlineValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Terminal)
            .MaximumLength(20);

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.");
    }
}
