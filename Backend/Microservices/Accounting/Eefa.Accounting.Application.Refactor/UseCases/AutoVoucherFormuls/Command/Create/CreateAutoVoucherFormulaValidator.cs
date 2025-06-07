using FluentValidation;

public class CreateAutoVoucherFormulaValidator : AbstractValidator<CreateAutoVoucherFormulaCommand>
{
    public CreateAutoVoucherFormulaValidator()
    {
        RuleFor(x => x.VoucherTypeId)
            .NotEmpty().WithMessage("کد نوع سند مقصد نمی تواند خالی باشد.")
            .NotEqual(0).WithMessage("کد نوع سند مقصد نمی تواند صفر باشد.");

        RuleFor(x => x.OrderIndex)
            .NotEmpty().WithMessage("ترتیب آرتیکل سند حسابداری نمی تواند خالی باشد.");

        RuleFor(x => x.DebitCreditStatus)
            .NotEmpty().WithMessage("وضعیت مانده حساب نمی تواند خالی باشد.");

        RuleFor(x => x.AccountHeadId)
            .NotEmpty().WithMessage("کد سطح نمی تواند خالی باشد.")
            .NotEqual(0).WithMessage("کد سطح نمی تواند صفر باشد.");

        When(y => y.RowDescription != null && y.RowDescription != string.Empty, () =>
        {
            RuleFor(x => x.RowDescription)
                .MaximumLength(200).WithMessage("طول توضیحات سطر بیش از حد تایین شده است.");
        });

        When(y => y.GroupBy != null && y.GroupBy != string.Empty, () =>
        {
            RuleFor(x => x.GroupBy)
                .MaximumLength(200).WithMessage("طول دسته بندی بر اساس بیش از حد تایین شده است.");
        });
    }
}