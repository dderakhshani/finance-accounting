using FluentValidation;

public class DeleteMenuItemValidator : AbstractValidator<DeleteMenuItemCommand>
{
    public DeleteMenuItemValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
            .GreaterThan(0).WithMessage("شناسه نمی تواند برابر یا کمتر از 0 باشد.");
    }
}