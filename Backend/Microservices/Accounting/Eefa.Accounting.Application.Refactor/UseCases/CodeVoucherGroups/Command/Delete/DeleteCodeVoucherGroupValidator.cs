using FluentValidation;
using System.Threading.Tasks;
using System.Threading;

public class DeleteCodeVoucherGroupValidator : AbstractValidator<DeleteCodeVoucherGroupCommand>
{
    public IUnitOfWork _unitOfWork { get; }
    public DeleteCodeVoucherGroupValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork= unitOfWork;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
            .NotEqual(0).WithMessage("شناسه نمیتواند صفر باشد.")
            .MustAsync(isInUs).WithMessage("شناسه مورد نظر یافت نشد");
    }
    public async Task<bool> isInUs(int id, CancellationToken cancellationToken)
    {
        if ((await _unitOfWork.CodeVoucherGroups.ExistsAsync(x => x.Id == id)) && 
            (await _unitOfWork.VouchersHeads.ExistsAsync(x => x.CodeVoucherGroupId == id)))
        {
            return true;
        }
        return false;
    }
}