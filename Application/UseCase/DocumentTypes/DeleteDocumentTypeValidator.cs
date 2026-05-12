using FluentValidation;

namespace Application.UseCase.DocumentTypes;

public sealed class DeleteDocumentTypeValidator : AbstractValidator<DeleteDocumentType>
{
    public DeleteDocumentTypeValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id es obligatorio.");
    }
}
