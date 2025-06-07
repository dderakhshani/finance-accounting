using FluentValidation;

public class CreateBranchValidator : AbstractValidator<CreateBranchCommand>
{
    public CreateBranchValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان نمی تواند خالی باشد.")
            .MaximumLength(50).WithMessage("عنوان نمی تواند بیشتر از 50 کاراکتر باشد.");

        When(y => (y.ParentId != null) && (y.ParentId > 0), () =>
        {
            RuleFor(t => t.ParentId)
                .GreaterThan(0).WithMessage("کد والد نمی تواند برابر یا کمتر از 0 باشد.");
        });

        When(y => (y.Lat != null) && (y.Lat > 0), () =>
        {
            //RuleFor(t => t.Lat)
            //.GreaterThan(0).WithMessage("نمی تواند برابر یا کمتر از 0 باشد.");
        });

        When(y => (y.Lng != null) && (y.Lng > 0), () =>
        {
            //RuleFor(t => t.Lng)
            //.GreaterThan(0).WithMessage("نمی تواند برابر یا کمتر از 0 باشد.");
        });
    }
}