using FluentValidation;
using System.Threading.Tasks;
using System.Threading;

public class DeleteCodeVoucherExtendTypeValidator : AbstractValidator<DeleteCodeVoucherExtendTypeCommand>
{
    public IUnitOfWork _unitOfWork { get; }
    public DeleteCodeVoucherExtendTypeValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork= unitOfWork;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
            .NotEqual(0).WithMessage("شناسه نمیتواند صفر باشد.")
            .MustAsync(idMustExist).WithMessage("شناسه مورد نظر یافت نشد");
    }
    public async Task<bool> idMustExist(int id, CancellationToken cancellationToken)
    {
        return await _unitOfWork.CodeVoucherExtendTypes.ExistsAsync(x => x.Id == id);
    }
}