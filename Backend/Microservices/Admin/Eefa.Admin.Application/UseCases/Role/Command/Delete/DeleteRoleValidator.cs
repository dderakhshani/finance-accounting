using FluentValidation;

namespace Eefa.Admin.Application.CommandQueries.Role.Command.Delete
{
    public class DeleteRoleValidator : AbstractValidator<DeleteRoleCommand>
    {
        public DeleteRoleValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
                .GreaterThan(0).WithMessage("شناسه نمی تواند برابر یا کمتر از 0 باشد.");
        }
    }
}