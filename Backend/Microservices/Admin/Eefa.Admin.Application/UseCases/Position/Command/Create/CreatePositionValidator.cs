using FluentValidation;

namespace Eefa.Admin.Application.CommandQueries.Position.Command.Create
{
    public class CreatePositionValidator : AbstractValidator<CreatePositionCommand>
    {
        public CreatePositionValidator()
        {
            When(y => (y.ParentId != null) && (y.ParentId > 0), () =>
            {
                RuleFor(t => t.ParentId)
                    .GreaterThan(0).WithMessage("کد والد نمی تواند برابر یا کمتر از 0 باشد.");
            });

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان نمی تواند خالی باشد.")
                .MaximumLength(50).WithMessage("عنوان نمی تواند بیشتر از 50 کاراکتر باشد.");
        }
    }
}