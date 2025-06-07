using FluentValidation;

public class UpdatePermissionValidator : AbstractValidator<UpdatePermissionCommand>
{
    public UpdatePermissionValidator()
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
            .MaximumLength(50).WithMessage("عنوان نمی تواند بیشتر از 50 کاراکتر باشد.");

        RuleFor(x => x.UniqueName)
            .NotEmpty().WithMessage("نام یکتا نمی تواند خالی باشد.")
            .MaximumLength(50).WithMessage("نام یکتا نمی تواند بیشتر از 50 کاراکتر باشد.");

        RuleFor(x => x.SubSystem)
            .NotEmpty().WithMessage("زیر سیستم نمی تواند خالی باشد.");

        RuleFor(x => x.TableName)
            .NotEmpty().WithMessage("نام جدول نمی تواند خالی باشد.");

        //RuleFor(x => x.TermsOfAccesses)
        //    .MustAsync(ValidateTermsOfAccesses).WithMessage("");
    }

    //private async Task<bool> ValidateTermsOfAccesses(List<TermsOfAccess> list, CancellationToken token)
    //{
    //    throw new NotImplementedException();
    //}
}