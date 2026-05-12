using FluentValidation;

namespace Application.UseCase.Regions;

public sealed class DeleteRegionValidator : AbstractValidator<DeleteRegion>
{
    public DeleteRegionValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id es obligatorio.");
    }
}
