using FluentValidation;

namespace Tutor.Application.Features.Heroes.DeleteHero;

public class DeleteHeroValidator : AbstractValidator<DeleteHeroRequest>
{

    public DeleteHeroValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}