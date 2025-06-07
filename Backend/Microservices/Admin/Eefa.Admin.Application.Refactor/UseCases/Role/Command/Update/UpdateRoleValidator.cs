using FluentValidation;

public class UpdateRoleValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
            .GreaterThan(0).WithMessage("شناسه نمی تواند برابر یا کمتر از 0 باشد.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان نمی تواند خالی باشد.")
            .MaximumLength(50).WithMessage("عنوان نمی تواند بیشتر از 50 کاراکتر باشد.");

        When(y => !string.IsNullOrEmpty(y.UniqueName), () =>
        {
            RuleFor(t => t.UniqueName)
                .MaximumLength(50).WithMessage("نام یکتا نمی تواند بیشتر از 50 کاراکتر باشد.");
        });

        When(y => !string.IsNullOrEmpty(y.Description), () =>
        {
            RuleFor(t => t.Description)
                .MaximumLength(50).WithMessage("توضیحات نمی تواند بیشتر از 50 کاراکتر باشد.");
        });

        When(y => (y.ParentId != null) && (y.ParentId > 0), () =>
        {
            RuleFor(t => t.ParentId)
                .GreaterThan(0).WithMessage("کد والد نمی تواند برابر یا کمتر از 0 باشد.");
        });

        //RuleFor(x => x.PermissionsId)
        //    .MustAsync(ValidatePermissionIds).WithMessage("");
    }

    //private async Task<bool> ValidatePermissionIds(IList<int> list, CancellationToken token)
    //{
    //    throw new NotImplementedException();
    //}
}