using FluentValidation;

namespace Eefa.Admin.Application.CommandQueries.User.Command.Delete
{
    public class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
                .GreaterThan(0).WithMessage("شناسه نمی تواند برابر یا کمتر از 0 باشد.");
        }
    }
}