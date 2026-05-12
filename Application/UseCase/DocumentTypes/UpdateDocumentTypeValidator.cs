using FluentValidation;

namespace Application.UseCase.DocumentTypes;

public sealed class UpdateDocumentTypeValidator : AbstractValidator<UpdateDocumentType>
{
    public UpdateDocumentTypeValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id es obligatorio.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 caracteres.")
            .MaximumLength(50).WithMessage("El nombre no puede superar los 50 caracteres.")
            .Matches(@"^[\p{L}\s]+$").WithMessage("El nombre solo puede contener letras y espacios.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("El codigo es obligatorio.")
            .MaximumLength(10).WithMessage("El codigo no puede superar los 10 caracteres.")
            .Matches(@"^[A-Za-z]+$").WithMessage("El codigo solo puede contener letras.");
    }
}
