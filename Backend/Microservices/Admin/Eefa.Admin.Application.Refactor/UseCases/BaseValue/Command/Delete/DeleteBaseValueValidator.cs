using FluentValidation;

public class DeleteBaseValueValidator : AbstractValidator<DeleteBaseValueCommand>
{
    public DeleteBaseValueValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
            .GreaterThan(0).WithMessage("شناسه نمی تواند برابر یا کمتر از 0 باشد.");
    }
}