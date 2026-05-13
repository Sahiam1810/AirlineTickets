using FluentValidation;

namespace Application.UseCase.ReservationStatuses;

public sealed class CreateReservationStatusValidator : AbstractValidator<CreateReservationStatus>
{
    public CreateReservationStatusValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Name)
            .Must(name => !string.IsNullOrWhiteSpace(name) && name.Trim().All(char.IsLetter))
            .WithMessage("El nombre solo puede contener letras.");
    }
}
