using FluentValidation;

namespace Application.UseCase.ReservationStatuses;

public sealed class UpdateReservationStatusValidator : AbstractValidator<UpdateReservationStatus>
{
    public UpdateReservationStatusValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("El Id es obligatorio.");
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Name)
            .Must(name => !string.IsNullOrWhiteSpace(name) && name.Trim().All(char.IsLetter))
            .WithMessage("El nombre solo puede contener letras.");
    }
}
