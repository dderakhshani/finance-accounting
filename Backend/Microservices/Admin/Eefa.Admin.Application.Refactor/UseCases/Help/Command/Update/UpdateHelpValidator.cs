using FluentValidation;

public class UpdateHelpValidator : AbstractValidator<UpdateHelpCommand>
{
    public UpdateHelpValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
            .GreaterThan(0).WithMessage("شناسه نمی تواند برابر یا کمتر از 0 باشد.");

        RuleFor(x => x.MenuItemId)
            .NotEmpty().WithMessage("عنوان صفحه نمی تواند خالی باشد.")
            .GreaterThan(0).WithMessage("عنوان صفحه نمی تواند برابر یا کمتر از 0 باشد.");

        RuleFor(x => x.Contents)
            .NotEmpty().WithMessage("محتوا نمی تواند خالی باشد.");
    }
}