using FluentValidation;

namespace Application.UseCase.AircraftManufacturers;

public sealed class CreateAircraftManufacturerValidator : AbstractValidator<CreateAircraftManufacturer>
{
    public CreateAircraftManufacturerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Manufacturer name is required.")
            .MaximumLength(100);

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country name is required.")
            .MaximumLength(100);
    }
}
