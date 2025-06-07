using FluentValidation;

public class UpdateBaseValueTypeValidator : AbstractValidator<UpdateBaseValueTypeCommand>
{
    public UpdateBaseValueTypeValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
            .GreaterThan(0).WithMessage("شناسه نمی تواند برابر یا کمتر از 0 باشد.");

        When(y => (y.ParentId != null) && (y.ParentId > 0), () =>
        {
            RuleFor(t => t.ParentId)
                .GreaterThan(0).WithMessage("کد والد نمی تواند برابر یا کمتر از 0 باشد.");
        });

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان نمی تواند خالی باشد.")
            .MaximumLength(250).WithMessage("عنوان نمی تواند بیشتر از 250 کاراکتر باشد.");

        RuleFor(x => x.UniqueName)
            .NotEmpty().WithMessage("نام اختصاصی نمی تواند خالی باشد.")
            .MaximumLength(50).WithMessage("نام اختصاصی نمی تواند بیشتر از 50 کاراکتر باشد.");

        When(y => !string.IsNullOrEmpty(y.GroupName), () =>
        {
            RuleFor(t => t.GroupName)
                .MaximumLength(50).WithMessage("نام گروه نمی تواند بیشتر از 50 کاراکتر باشد.");
        });

        When(y => !string.IsNullOrEmpty(y.SubSystem), () =>
        {
            RuleFor(t => t.SubSystem)
                .MaximumLength(50).WithMessage("زیر سیستم نمی تواند بیشتر از 50 کاراکتر باشد.");
        });
    }
}