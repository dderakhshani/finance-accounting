using FluentValidation;

public class UpdateAttachmentValidator : AbstractValidator<UpdateAttachmentCommand>
{
    public UpdateAttachmentValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
            .GreaterThan(0).WithMessage("شناسه نمی تواند برابر یا کمتر از 0 باشد.");

        RuleFor(x => x.LanguageId)
            .NotEmpty().WithMessage("کد زبان نمی تواند خالی باشد.")
            .GreaterThan(0).WithMessage("کد زبان وارد شده نمی تواند برابر یا کمتر از 0 باشد.");

        RuleFor(x => x.TypeBaseId)
            .NotEmpty().WithMessage("اطلاعات پایه نمی تواند خالی باشد.")
            .GreaterThan(0).WithMessage("اطلاعات پایه وارد شده نامعتبر میباشد.");

        RuleFor(x => x.Extention)
            .NotEmpty().WithMessage("افزونه نمی تواند خالی باشد.")
            .MaximumLength(20).WithMessage("افزونه نباید بیشتر از 20 کاراکتر باشد.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان نمی تواند خالی باشد.")
            .MaximumLength(250).WithMessage("عنوان نباید بیشتر از 250 کاراکتر باشد.");

        When(y => !string.IsNullOrEmpty(y.Description), () =>
        {
            RuleFor(t => t.Description)
                .MaximumLength(3000).WithMessage("شرح نباید بیشتر از 3000 کاراکتر باشد.");
        });

        When(y => !string.IsNullOrEmpty(y.KeyWords), () =>
        {
            RuleFor(t => t.KeyWords)
                .MaximumLength(250).WithMessage("کلمات کلیدی نباید بیشتر از 250 کاراکتر باشد.");
        });

        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("URL نباید خالی باشد.")
            .MaximumLength(1000).WithMessage("URL نمی تواند بیشتر از1000 کاراکتر باشد");
    }
}