using FluentValidation;
using System.Threading.Tasks;
using System.Threading;

public class DeleteAccountReferenceValidator : AbstractValidator<DeleteAccountReferenceCommand>
{
    public IUnitOfWork _unitOfWork { get; }
    public DeleteAccountReferenceValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork= unitOfWork;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
            .NotEqual(0).WithMessage("شناسه نمیتواند صفر باشد.")
            .MustAsync(IsAccountReferenceInUse).WithMessage("شناسه مورد نظر یافت نشد");
    }

    public async Task<bool> IsAccountReferenceInUse(int id, CancellationToken cancellationToken)
    {
       return !await _unitOfWork.VouchersDetails
              .ExistsAsync(x => x.ReferenceId1 == id ||
                                x.ReferenceId2 == id ||
                                x.ReferenceId3 == id);
    }
}