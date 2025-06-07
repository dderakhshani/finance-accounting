using FluentValidation;
using System.Threading.Tasks;
using System.Threading;

public class CreateAccountReferenceValidator : AbstractValidator<CreateAccountReferenceCommand>
{
    public IUnitOfWork _unitOfWork { get; }

    public CreateAccountReferenceValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork= unitOfWork;

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان نمی تواند خالی باشد.")
            .MaximumLength(200).WithMessage("طول عنوان بیشتر از حد تایین شده است.")
            .MustAsync(beUniqueTitle).WithMessage("عنوان تکراری است.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("کد نمی تواند خالی باشد.")
            .MaximumLength(200).WithMessage("طول کد بیشتر از حد تایین شده است.")
            .MustAsync(beUniqueCode).WithMessage("کد تکراری است.");

        When(x => x.Description != null, () =>
        {
            RuleFor(x => x.Description)
                .MaximumLength(250).WithMessage("طول شرح بیشتر از حد تایین شده است.");
        });
    }
    public async Task<bool> beUniqueTitle(string title, CancellationToken cancellationToken)
    {
        return !await _unitOfWork.AccountReferences.ExistsAsync(x => x.Title == title);
    }
    public async Task<bool> beUniqueCode(string code, CancellationToken cancellationToken)
    {
        return !await _unitOfWork.AccountReferences.ExistsAsync(x => x.Code == code);
    }
}