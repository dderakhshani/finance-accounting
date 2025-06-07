using FluentValidation;

namespace Eefa.Admin.Application.CommandQueries.Language.Command.Update
{
    public class UpdateLanguageValidator : AbstractValidator<UpdateLanguageCommand>
    {
        public UpdateLanguageValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
                .GreaterThan(0).WithMessage("شناسه نمی تواند برابر یا کمتر از 0 باشد.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان نمی تواند خالی باشد.")
                .MaximumLength(100).WithMessage("عنوان نمی تواند بیشتر از 100 کاراکتر باشد.");

            RuleFor(x => x.Culture)
                .NotEmpty().WithMessage("کد اختصاصی نمی تواند خالی باشد.")
                .MaximumLength(20).WithMessage("کد اختصاصی نمی تواند بیشتر از 20 کاراکتر باشد.");

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
                .NotEmpty().WithMessage("واحد پول نمی تواند خالی باشد.")
                .GreaterThan(0).WithMessage("واحد پول نمی تواند برابر یا کمتر از 0 باشد.");
        }
    }
}