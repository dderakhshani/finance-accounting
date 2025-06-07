using FluentValidation;

public class CreateLanguageValidator : AbstractValidator<CreateLanguageCommand>
{
    public CreateLanguageValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان نمی تواند خالی باشد.")
            .MaximumLength(100).WithMessage("عنوان نمی تواند بیشتر از 100 کاراکتر باشد.");

        RuleFor(x => x.Culture)
            .NotEmpty().WithMessage("نماد نمی تواند خالی باشد.")
            .MaximumLength(20).WithMessage("نماد نمی تواند بیشتر از 20 کاراکتر باشد.");

        When(y => !string.IsNullOrEmpty(y.SeoCode), () =>
        {
            RuleFor(t => t.SeoCode)
                .MaximumLength(2).WithMessage("کد سئو نمی تواند بیشتر از 2 کاراکتر باشد.");
        });

        When(y => !string.IsNullOrEmpty(y.FlagImageUrl), () =>
        {
            RuleFor(t => t.FlagImageUrl)
                .MaximumLength(500).WithMessage("نماد پرچم کشور نمی تواند بیشتر از 500 کاراکتر باشد.");
        });

        RuleFor(x => x.DirectionBaseId)
            .NotEmpty().WithMessage("لطفا یک جهت نوشتاری انتخاب کنید.")
            .GreaterThan(0).WithMessage("کد جهت نوشتاری نمی تواند کمتر یا برابر با 0 باشد.");

        RuleFor(x => x.DefaultCurrencyBaseId)
            .NotEmpty().WithMessage("واحد پول پیش فرض نمی تواند خالی باشد.")
            .GreaterThan(0).WithMessage("کد واحد پول پیش فرض نمی تواند کمتر یا برابر از 0 باشد.");
    }
}