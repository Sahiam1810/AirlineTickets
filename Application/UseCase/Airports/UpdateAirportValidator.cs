using FluentValidation;

namespace Application.UseCase.Airports;

public sealed class UpdateAirportValidator : AbstractValidator<UpdateAirport>
{
    public UpdateAirportValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Airport name is required.")
            .MaximumLength(150);

        RuleFor(x => x.IataCode)
            .NotEmpty().WithMessage("IATA code is required.")
            .Length(3);

        RuleFor(x => x.IcaoCode)
            .MaximumLength(4);

        RuleFor(x => x.CityId)
            .NotEmpty().WithMessage("City id is required.");
    }
}
