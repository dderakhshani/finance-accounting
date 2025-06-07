using FluentValidation;

public class CreateUnitValidator : AbstractValidator<CreateUnitCommand>
{
    public CreateUnitValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان نمی تواند خالی باشد.")
            .MaximumLength(50).WithMessage("عنوان نمی تواند بیشتر از 50 کاراکتر باشد.");

        When(y => (y.ParentId != null) && (y.ParentId > 0), () =>
        {
            RuleFor(t => t.ParentId)
                .GreaterThan(0).WithMessage("کد والد نمی تواند برابر یا کمتر از 0 باشد.");
        });

        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("کد شعبه نمی تواند خالی باشد.");

        //RuleFor(x => x.PositionIds)
        //    .MustAsync(ValidatePositionIds).WithMessage("");
    }

    //private async Task<bool> ValidatePositionIds(IList<int> list, CancellationToken token)
    //{
    //    throw new NotImplementedException();
    //}
}