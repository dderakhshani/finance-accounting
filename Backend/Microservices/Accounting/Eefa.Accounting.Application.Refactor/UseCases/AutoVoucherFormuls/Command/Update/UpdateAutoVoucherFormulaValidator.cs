using FluentValidation;
using System.Threading.Tasks;
using System.Threading;

public class UpdateAutoVoucherFormulaValidator : AbstractValidator<UpdateAutoVoucherFormulaCommand>
{
    public IUnitOfWork _unitOfWork { get; }
    public UpdateAutoVoucherFormulaValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork= unitOfWork;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
            .NotEqual(0).WithMessage("شناسه نمیتواند صفر باشد.")
            .MustAsync(idMustExist).WithMessage("شناسه مورد نظر یافت نشد");

        RuleFor(x => x.VoucherTypeId)
            .NotEmpty().WithMessage("کد نوع سند نمی تواند خالی باشد.")
            .NotEqual(0).WithMessage("کد نوع سند نمی تواند صفر باشد.");

        RuleFor(x => x.SourceVoucherTypeId)
            .NotEmpty().WithMessage("کد نوع سند مبدا نمی تواند خالی باشد.");

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

        When(y => y.GroupBy != null && y.RowDescription != string.Empty, () =>
        {
            RuleFor(x => x.GroupBy)
                .MaximumLength(200).WithMessage("طول دسته بندی بر اساس بیش از حد تایین شده است.");
        });
    }
    public async Task<bool> idMustExist(int id, CancellationToken cancellationToken)
    {
        return await _unitOfWork.AutoVoucherFormulas.ExistsAsync(x => x.Id == id);
    }
}