using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

public class CreateCodeRowDescriptionValidator : AbstractValidator<CreateCodeRowDescriptionCommand>
{
    public IUnitOfWork _unitOfWork { get; }
    public CreateCodeRowDescriptionValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork= unitOfWork;

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان نمی تواند خالی باشد.")
            .MaximumLength(200).WithMessage("طول عنوان بیشتر از حد تایین شده است.")
            .MustAsync(beUniqueTitle).WithMessage("عنوان تکراری است.");
    }
    public async Task<bool> beUniqueTitle(string title, CancellationToken cancellationToken)
    {
        return await _unitOfWork.CodeRowDescriptions.ExistsAsync(x => x.Title == title);
    }
}