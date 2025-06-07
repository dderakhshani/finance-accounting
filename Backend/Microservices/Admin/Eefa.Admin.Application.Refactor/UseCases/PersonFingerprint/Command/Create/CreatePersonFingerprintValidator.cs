using FluentValidation;

public class CreatePersonFingerprintValidator : AbstractValidator<CreatePersonFingerprintCommand>
{
    public CreatePersonFingerprintValidator()
    {
        RuleFor(x => x.PersonId)
            .NotEmpty().WithMessage("کد شخص نمی تواند خالی باشد.")
            .GreaterThan(0).WithMessage("کد شخص نمی تواند برابر یا کمتر از 0 باشد.");

        //RuleFor(x => x.FingerPrintPhoto)
        //    .MustAsync(ValidateFingerPrintPhoto).WithMessage("");

        RuleFor(x => x.FingerBaseId)
            .NotEmpty().WithMessage("شماره انگشت نمی تواند خالی باشد.")
            .GreaterThan(0).WithMessage("شماره انگشت نمی تواند برابر یا کمتر از 0 باشد.");

        RuleFor(x => x.FingerPrintTemplate)
            .NotEmpty().WithMessage("الگوی اثر انگشت نمی تواند خالی باشد.")
            .MaximumLength(2000).WithMessage("الگوی اثر انگشت نمی تواند بیشتر از 2000 کاراکتر باشد.");
    }

    //private async Task<bool> ValidateFingerPrintPhoto(IFormFile file, CancellationToken token)
    //{
    //    throw new NotImplementedException();
    //}
}