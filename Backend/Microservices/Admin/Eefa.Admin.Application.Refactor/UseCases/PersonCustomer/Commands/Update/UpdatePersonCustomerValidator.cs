using FluentValidation;

public class UpdatePersonCustomerValidator : AbstractValidator<UpdatePersonCustomerCommand>
{
    public UpdatePersonCustomerValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
            .GreaterThan(0).WithMessage("شناسه نمی تواند برابر یا کمتر از 0 باشد.");

        RuleFor(x => x.PersonId)
            .NotEmpty().WithMessage("کد شخص نمی تواند خالی باشد.")
            .GreaterThan(0).WithMessage("کد شخص نمی تواند برابر یا کمتر از 0 باشد.");

        RuleFor(x => x.CustomerTypeBaseId)
            .NotEmpty().WithMessage("گروه قیمتی نمی تواند خالی باشد.")
            .GreaterThan(0).WithMessage("گروه قیمتی نمی تواند برابر یا کمتر از 0 باشد.");

        RuleFor(x => x.CustomerCode)
            .NotEmpty().WithMessage("کد مشتری نمی تواند خالی باشد.")
            .MaximumLength(50).WithMessage("کد مشتری نمی تواند بیشتر از 50 کاراکتر باشد.");

        RuleFor(x => x.CurentExpertId)
            .NotEmpty().WithMessage("نمی تواند خالی باشد.")
            .GreaterThan(0).WithMessage("نمی تواند برابر یا کمتر از 0 باشد.");

        When(y => !string.IsNullOrEmpty(y.EconomicCode), () =>
        {
            RuleFor(t => t.EconomicCode)
                .MaximumLength(50).WithMessage("کد اقتصادی مشتری نمی تواند بیشتر از 50 کاراکتر باشد.");
        });

        When(y => !string.IsNullOrEmpty(y.Description), () =>
        {
            RuleFor(t => t.Description)
                .MaximumLength(500).WithMessage("توضیحات نمی تواند بیشتر از 500 کاراکتر باشد.");
        });
    }
}