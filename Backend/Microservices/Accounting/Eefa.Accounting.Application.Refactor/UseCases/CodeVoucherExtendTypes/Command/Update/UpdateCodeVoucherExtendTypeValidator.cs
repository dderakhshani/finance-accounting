using FluentValidation;
using System.Threading.Tasks;
using System.Threading;

public class UpdateCodeVoucherExtendTypeValidator : AbstractValidator<UpdateCodeVoucherExtendTypeCommand>
{
    public IUnitOfWork _unitOfWork { get; }
    public UpdateCodeVoucherExtendTypeValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork= unitOfWork;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
            .NotEqual(0).WithMessage("شناسه نمیتواند صفر باشد.")
            .MustAsync(idMustExist).WithMessage("شناسه مورد نظر یافت نشد");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان نمی تواند خالی باشد.")
            .MaximumLength(100).WithMessage("طول عنوان بیشتر از حد تایین شده است.");
    }
    public async Task<bool> idMustExist(int id, CancellationToken cancellationToken)
    {
        return await _unitOfWork.CodeVoucherExtendTypes.ExistsAsync(x => x.Id == id);
    }
}