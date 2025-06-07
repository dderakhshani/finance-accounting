using FluentValidation;

namespace Eefa.Admin.Application.CommandQueries.Permission.Command.Delete
{
    public class DeletePermissionValidator : AbstractValidator<DeletePermissionCommand>
    {
        public DeletePermissionValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
                .GreaterThan(0).WithMessage("شناسه نمی تواند برابر یا کمتر از 0 باشد.");
        }
    }
}