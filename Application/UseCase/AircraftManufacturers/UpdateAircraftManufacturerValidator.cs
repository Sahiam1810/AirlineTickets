using FluentValidation;

namespace Application.UseCase.AircraftManufacturers;

public sealed class UpdateAircraftManufacturerValidator : AbstractValidator<UpdateAircraftManufacturer>
{
    public UpdateAircraftManufacturerValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Manufacturer name is required.")
            .MaximumLength(100);

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country name is required.")
            .MaximumLength(100);
    }
}
