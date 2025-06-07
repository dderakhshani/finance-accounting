using FluentValidation;
using System.Threading.Tasks;
using System.Threading;

public class UpdateCodeVoucherGroupValidator : AbstractValidator<UpdateCodeVoucherGroupCommand>
{
    public IUnitOfWork _unitOfWork { get; }
    public UpdateCodeVoucherGroupValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork= unitOfWork;

        RuleFor(x => x)
            .MustAsync(beUniqueCode).WithMessage("کد تکراری است.")
            .MustAsync(beUniqueName).WithMessage("نام یکتا تکراری است");


        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
            .NotEqual(0).WithMessage("شناسه نمیتواند صفر باشد.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("کد نمی تواند خالی باشد.")
            .MaximumLength(50).WithMessage("تعداد کاراکتر های کد بیش اندازه است.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان نمی تواند خالی باشد.")
            .MaximumLength(100).WithMessage("تعداد کاراکتر های عنوان بیش اندازه است.");

        RuleFor(x => x.UniqueName)
            .NotEmpty().WithMessage("نام یکتا نمی تواند خالی باشد.");

        When(y => y.BlankDateFormula != null && y.BlankDateFormula != string.Empty, () =>
        {
            RuleFor(x => x.BlankDateFormula)
                .MaximumLength(300).WithMessage("تعداد کاراکتر های فرمول جایگزین خالی بودن بیش از اندازه است.");
        });

        When(y => y.ViewId != null, () =>
        {
            RuleFor(x => x.ViewId)
                .GreaterThan(0).WithMessage("کد گزارش وارد شده معتبر نیست.");
        });

        When(y => y.ExtendTypeId != null, () =>
        {
            RuleFor(x => x.ExtendTypeId)
                .GreaterThan(0).WithMessage("نوع افزونه وارد شده معتبر نیست.");
        });
    }
    public async Task<bool> beUniqueName(UpdateCodeVoucherGroupCommand codeVoucherGroup, CancellationToken cancellationToken)
    {
        return !await _unitOfWork.CodeVoucherGroups
                              .ExistsAsync(x => x.UniqueName == codeVoucherGroup.UniqueName
                                             && x.Id != codeVoucherGroup.Id);
    }
    public async Task<bool> beUniqueCode(UpdateCodeVoucherGroupCommand codeVoucherGroup, CancellationToken cancellationToken)
    {
        return !await _unitOfWork.CodeVoucherGroups
                             .ExistsAsync(x => x.Code == codeVoucherGroup.Code
                                            && x.Id != codeVoucherGroup.Id);
    }
}