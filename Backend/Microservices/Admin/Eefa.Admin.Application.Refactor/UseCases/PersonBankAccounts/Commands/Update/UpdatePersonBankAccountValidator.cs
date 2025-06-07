using FluentValidation;

public class UpdatePersonBankAccountValidator : AbstractValidator<UpdatePersonBankAccountCommand>
{
    public UpdatePersonBankAccountValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
            .GreaterThan(0).WithMessage("شناسه نمی تواند برابر یا کمتر از 0 باشد.");

        RuleFor(x => x.PersonId)
            .NotEmpty().WithMessage("کد شخص نمی تواند خالی باشد.")
            .GreaterThan(0).WithMessage("کد شخص نمی تواند برابر یا کمتر از 0 باشد.");

        When(y => (y.BankId != null) && (y.BankId > 0), () =>
        {
            RuleFor(t => t.BankId)
                .GreaterThan(0).WithMessage("کد بانک نمی تواند برابر یا کمتر از 0 باشد.");
        });

        When(y => !string.IsNullOrEmpty(y.BankBranchName), () =>
        {
            RuleFor(t => t.BankBranchName)
                .MaximumLength(50).WithMessage("نام شعبه بانک نمی تواند بیشتر از 50 کاراکتر باشد.");
        });

        RuleFor(x => x.AccountTypeBaseId)
            .NotEmpty().WithMessage("نوع حساب نمی تواند خالی باشد.")
            .GreaterThan(0).WithMessage("نوع حساب نمی تواند برابر یا کمتر از 0 باشد.");

        RuleFor(x => x.AccountNumber)
            .NotEmpty().WithMessage("شماره حساب نمی تواند خالی باشد.")
            .MaximumLength(50).WithMessage("شماره حساب نمی تواند بیشتر از 50 کاراکتر باشد.");

        When(y => !string.IsNullOrEmpty(y.Description), () =>
        {
            RuleFor(t => t.Description)
                .MaximumLength(500).WithMessage("توضیحات نمی تواند بیشتر از 500 کاراکتر باشد.");
        });
    }
}