using FluentValidation;

namespace Application.UseCase.People;

public sealed class CreatePersonValidator : AbstractValidator<CreatePerson>
{
    public CreatePersonValidator()
    {
        RuleFor(x => x.DocumentTypeId)
            .GreaterThan(0).WithMessage("El tipo de documento es obligatorio.");

        RuleFor(x => x.DocumentNumber)
            .NotEmpty().WithMessage("El numero de documento es obligatorio.")
            .MaximumLength(30).WithMessage("El numero de documento no puede superar los 30 caracteres.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Los nombres son obligatorios.")
            .MaximumLength(100).WithMessage("Los nombres no pueden superar los 100 caracteres.")
            .Matches(@"^[\p{L}\s]+$").WithMessage("Los nombres solo pueden contener letras y espacios.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Los apellidos son obligatorios.")
            .MaximumLength(100).WithMessage("Los apellidos no pueden superar los 100 caracteres.")
            .Matches(@"^[\p{L}\s]+$").WithMessage("Los apellidos solo pueden contener letras y espacios.");

        RuleFor(x => x.Gender)
            .Must(g => string.IsNullOrWhiteSpace(g) || new[] { "M", "F", "N" }.Contains(g.Trim().ToUpperInvariant()))
            .WithMessage("El genero debe ser M, F o N.");
    }
}
