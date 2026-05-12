using FluentValidation;

namespace Application.UseCase.AircraftModels;

public sealed class UpdateAircraftModelValidator : AbstractValidator<UpdateAircraftModel>
{
    public UpdateAircraftModelValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.ManufacturerId)
            .NotEmpty().WithMessage("Manufacturer id is required.");

        RuleFor(x => x.ModelName)
            .NotEmpty().WithMessage("Model name is required.")
            .MaximumLength(100);

        RuleFor(x => x.MaxCapacity)
            .GreaterThan(0).WithMessage("Max capacity must be greater than 0.");

        RuleFor(x => x.MaxTakeoffWeightKg)
            .GreaterThan(0).When(x => x.MaxTakeoffWeightKg.HasValue);

        RuleFor(x => x.FuelConsumptionKgPerHour)
            .GreaterThan(0).When(x => x.FuelConsumptionKgPerHour.HasValue);

        RuleFor(x => x.CruiseSpeedKmh)
            .GreaterThan(0).When(x => x.CruiseSpeedKmh.HasValue);

        RuleFor(x => x.CruiseAltitudeFt)
            .GreaterThan(0).When(x => x.CruiseAltitudeFt.HasValue);
    }
}
