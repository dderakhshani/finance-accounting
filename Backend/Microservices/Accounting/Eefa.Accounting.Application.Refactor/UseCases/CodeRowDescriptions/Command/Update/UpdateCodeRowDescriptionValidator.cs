using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

public class UpdateCodeRowDescriptionValidator : AbstractValidator<UpdateCodeRowDescriptionCommand>
{
    public IUnitOfWork _unitOfWork { get; }
    public UpdateCodeRowDescriptionValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork= unitOfWork;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
            .NotEqual(0).WithMessage("شناسه نمیتواند صفر باشد.")
            .MustAsync(idMustExist).WithMessage("شناسه مورد نظر یافت نشد");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان نمی تواند خالی باشد.")
            .MustAsync(beUniqueTitle).WithMessage("عنوان تکراری است.")
            .MaximumLength(200).WithMessage("طول عنوان بیشتر از حد تایین شده است.");
    }

    public async Task<bool> idMustExist(int id, CancellationToken cancellationToken)
    {
        return await _unitOfWork.CodeRowDescriptions.ExistsAsync(x => x.Id == id);
    }

    public async Task<bool> beUniqueTitle(string title, CancellationToken cancellationToken)
    {
        return await _unitOfWork.CodeRowDescriptions.ExistsAsync(x => x.Title == title);
    }
}