using FluentValidation;
using System.Threading.Tasks;
using System.Threading;

public class DeleteAccountReferencesGroupValidator : AbstractValidator<DeleteAccountReferencesGroupCommand>
{
    public IUnitOfWork _unitOfWork { get; }
    public DeleteAccountReferencesGroupValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork= unitOfWork;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
            .NotEqual(0).WithMessage("شناسه نمیتواند صفر باشد.")
            .MustAsync(isReferenceGroupInUse).WithMessage("شناسه مورد نظر یافت نشد");
    }
    public async Task<bool> isReferenceGroupInUse(int id, CancellationToken cancellationToken)
    {
        return !await _unitOfWork.AccountReferencesRelReferencesGroups
               .ExistsAsync(x => x.ReferenceGroupId == id);
    }
}