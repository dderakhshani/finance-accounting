using FluentValidation;

public class UpdatePersonValidator : AbstractValidator<UpdatePersonCommand>
{
    public UpdatePersonValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
            .GreaterThan(0).WithMessage("شناسه نمی تواند برابر یا کمتر از 0 باشد.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("نام نمی تواند خالی باشد.")
            .MaximumLength(100).WithMessage("نام نمی تواند بیشتر از 100 کاراکتر باشد.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("نام خانوادگی نمی تواند خالی باشد.")
            .MaximumLength(100).WithMessage("نام خانوادگی نمی تواند بیشتر از 100 کاراکتر باشد.");

        When(y => !string.IsNullOrEmpty(y.FatherName), () =>
        {
            RuleFor(t => t.FatherName)
                .MaximumLength(100).WithMessage("نام پدر نمی تواند بیشتر از 100 کاراکتر باشد.");
        });

        RuleFor(x => x.NationalNumber)
            .NotEmpty().WithMessage("کد ملی نمی تواند خالی باشد.")
            .MaximumLength(50).WithMessage("کد ملی نمی تواند بیشتر از 50 کاراکتر باشد.");

        RuleFor(x => x.EconomicCode)
            .NotEmpty().WithMessage("کد اقتصادی/ کد مشارکت مدنی نمی تواند خالی باشد.")
            .MaximumLength(50).WithMessage("کد اقتصادی/ کد مشارکت مدنی نمی تواند بیشتر از 50 کاراکتر باشد.");

        When(y => !string.IsNullOrEmpty(y.IdentityNumber), () =>
        {
            RuleFor(t => t.IdentityNumber)
                .MaximumLength(50).WithMessage("شماره شناسنامه نمی تواند بیشتر از 50 کاراکتر باشد.");
        });

        When(y => !string.IsNullOrEmpty(y.InsuranceNumber), () =>
        {
            RuleFor(t => t.InsuranceNumber)
                .MaximumLength(50).WithMessage("شماره بیمه نمی تواند بیشتر از 50 کاراکتر باشد.");
        });

        When(y => !string.IsNullOrEmpty(y.Email), () =>
        {
            RuleFor(t => t.Email)
                .MaximumLength(320).WithMessage("ایمیل نمی تواند بیشتر از 320 کاراکتر باشد.");
        });

        When(y => (y.BirthPlaceCountryDivisionId != null) && (y.BirthPlaceCountryDivisionId > 0), () =>
        {
            RuleFor(t => t.BirthPlaceCountryDivisionId)
                .GreaterThan(0).WithMessage("شهر نمی تواند برابر یا کمتر از 0 باشد.");
        });

        RuleFor(x => x.GenderBaseId)
            .NotEmpty().WithMessage("جنسیت نمی تواند خالی باشد.")
            .GreaterThan(0).WithMessage("کد جنسیت نمی تواند برابر یا کمتر از 0 باشد.");

        When(y => (y.LegalBaseId != null) && (y.LegalBaseId > 0), () =>
        {
            RuleFor(t => t.LegalBaseId)
                .GreaterThan(0).WithMessage("نوع شخص نمی تواند برابر یا کمتر از 0 باشد.");
        });

        When(y => (y.GovernmentalBaseId != null) && (y.GovernmentalBaseId > 0), () =>
        {
            RuleFor(t => t.GovernmentalBaseId)
                .GreaterThan(0).WithMessage("کد ماهیت نمی تواند برابر یا کمتر از 0 باشد.");
        });

        RuleFor(x => x.ProfileImageReletiveAddress)
            .NotEmpty().WithMessage("نمی تواند خالی باشد.");

        RuleFor(x => x.SignatureImageReletiveAddress)
            .NotEmpty().WithMessage("نمی تواند خالی باشد.");
    }
}