using FluentValidation;

namespace Eefa.Admin.Application.UseCases.Help.Command.Delete
{
    public class DeleteHelpValidator : AbstractValidator<DeleteHelpCommand>
    {
        public DeleteHelpValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
                .GreaterThan(0).WithMessage("شناسه نمی تواند برابر یا کمتر از 0 باشد.");
        }
    }
}