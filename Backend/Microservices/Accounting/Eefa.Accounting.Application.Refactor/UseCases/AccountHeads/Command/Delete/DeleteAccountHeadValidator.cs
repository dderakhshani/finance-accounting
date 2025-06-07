using FluentValidation;
using System.Threading.Tasks;
using System.Threading;

public class DeleteAccountHeadValidator : AbstractValidator<DeleteAccountHeadCommand>
{
    public IUnitOfWork _unitOfWork { get; }
    public DeleteAccountHeadValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork= unitOfWork;

        When(x => x.ForceDeactive == false, () =>
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
                .NotEqual(0).WithMessage("شناسه نمیتواند صفر باشد.")
                .MustAsync(IsAccountHeadInUse).WithMessage("شناسه موردنظر استفاده شده است.")
                .MustAsync(IsAccountHeadInUseInCurrentYear).WithMessage("شناسه موردنظر در سال جاری استفاده شده است.");
        }).Otherwise(() =>
        {
            RuleFor(x => x.Id)
                    .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
                    .NotEqual(0).WithMessage("شناسه نمیتواند صفر باشد.")
                    .MustAsync(IsAccountHeadInUseInCurrentYear).WithMessage("شناسه موردنظر در سال جاری استفاده شده است.");
        });
    }
    public async Task<bool> IsAccountHeadInUseInCurrentYear(int id, CancellationToken token)
    {
        var currentYear = await _unitOfWork.Years.GetAsync(x => x.IsCurrentYear);

        return await _unitOfWork.VouchersDetails
                    .ExistsAsync(x => x.VoucherDate >= currentYear.FirstDate &&
                                      x.VoucherDate <= currentYear.LastDate &&
                                      x.AccountHeadId == id);
    }
    public async Task<bool> IsAccountHeadInUse(int id, CancellationToken cancellationToken)
    {
        var currentYear = await _unitOfWork.Years.GetAsync(x => x.IsCurrentYear);

        return await _unitOfWork.VouchersDetails
                    .ExistsAsync(x => x.VoucherDate >= currentYear.FirstDate &&
                                      x.VoucherDate <= currentYear.LastDate &&
                                      x.AccountHeadId == id);
    }
}