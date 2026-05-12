using FluentValidation;

namespace Application.UseCase.Airlines;

public sealed class UpdateAirlineValidator : AbstractValidator<UpdateAirline>
{
    public UpdateAirlineValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Airline name is required.")
            .MaximumLength(150);

        RuleFor(x => x.IataCode)
            .NotEmpty().WithMessage("IATA code is required.")
            .MaximumLength(3);

        RuleFor(x => x.CountryId)
            .NotEmpty().WithMessage("Country id is required.");
    }
}
