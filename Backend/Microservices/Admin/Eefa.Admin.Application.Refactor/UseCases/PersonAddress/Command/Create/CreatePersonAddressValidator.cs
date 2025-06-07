using FluentValidation;

public class CreatePersonAddressValidator : AbstractValidator<CreatePersonAddressCommand>
{
    public CreatePersonAddressValidator()
    {
        RuleFor(x => x.PersonId)
            .NotEmpty().WithMessage("کد شخص نمی تواند خالی باشد.")
            .GreaterThan(0).WithMessage("کد شخص نمی تواند برابر یا کمتر از 0 باشد.");

        RuleFor(x => x.TypeBaseId)
            .NotEmpty().WithMessage("عنوان نمی تواند خالی باشد.")
            .GreaterThan(0).WithMessage("عنوان نمی تواند برابر یا کمتر از 0 باشد.");

        When(y => !string.IsNullOrEmpty(y.Address), () =>
        {
            RuleFor(t => t.Address)
                .MaximumLength(500).WithMessage("آدرس نمی تواند بیشتر از 500 کاراکتر باشد.");
        });

        When(y => (y.CountryDivisionId != null) && (y.CountryDivisionId > 0), () =>
        {
            RuleFor(t => t.CountryDivisionId)
                .GreaterThan(0).WithMessage("کد شهرستان نمی تواند برابر یا کمتر از 0 باشد.");
        });

        //RuleFor(x => x.Mobiles)
        //    .MustAsync(ValidateMobiles).WithMessage("");

        When(y => !string.IsNullOrEmpty(y.PostalCode), () =>
        {
            RuleFor(t => t.PostalCode)
                .MaximumLength(10).WithMessage("کد پستی نمی تواند بیشتر از 10 کاراکتر باشد.");
        });
    }

    //private async Task<bool> ValidateMobiles(ICollection<PhoneNumber> collection, CancellationToken token)
    //{
    //    throw new NotImplementedException();
    //}
}