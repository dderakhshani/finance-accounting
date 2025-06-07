using FluentValidation;

namespace Eefa.Admin.Application.CommandQueries.Year.Command.Delete
{
    public class DeleteYearValidator : AbstractValidator<DeleteYearCommand>
    {
        public DeleteYearValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
                .GreaterThan(0).WithMessage("شناسه نمی تواند برابر یا کمتر از 0 باشد.");
        }
    }
}